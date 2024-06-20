namespace TestWPFApp
{
    public class PuzzleSolver
    {
        private const int XValue = 1;
        private const int DOTValue = 0;
        int _maxCoordinateX;
        int _maxCoordinateY;

        private List<Position> _boardPositions = new();
        private List<Position> _currentPiecePositions = new();
        private bool _puzzleSolved = false;

        /// <summary>
        /// My initial solution was to simply run through every possible combination of piece placed on the board
        /// Though this worked on the very first puzzle
        /// It ran into trouble for every other puzzle
        /// Even more problematic, it seems my solution was far too slow
        /// I underestimated how many possible combinations there were, and from the second puzzle on,
        /// Not only did it take several minutes for a solution to arrive, it did not work in the first place.
        /// However, I was unfortunately unable to really think of another way to solve this puzzle
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public string SolvePuzzle(Inputs inputs)
        {
            //First, convert all pieces to 2D arrays
            List<int[,]> convertedPieces = ConvertPieces(inputs.Pieces);

            if (!convertedPieces.Any())
                return "No pieces found in file";

            //Get all possible positions for this board
            GetAllPossiblePositions(inputs.Board);

            _maxCoordinateX = inputs.Board.GetLength(0) - 1;
            _maxCoordinateY = inputs.Board.GetLength(1) - 1;

            int piecesPlaced = 0;
            PlacePieces(ref piecesPlaced, convertedPieces, inputs.Board);

            if (!_puzzleSolved)
                return "No solution found";

            string solution = CreateSolutionString();

            return solution;
        }

        /// <summary>
        /// In order to process the pieces set in the file, I convert them to a 2D array.
        /// This would easily allow them to be placed on the Board, also converted to a 2D array
        /// </summary>
        /// <param name="pieces"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private List<int[,]> ConvertPieces(List<string> pieces)
        {
            List<int[,]> convertedPieces = new();

            foreach (string piece in pieces)
            {
                //split piece string into rows
                string[] piecesArray = piece.Split(",");

                //All piece rows have the same length
                int pieceRows = piecesArray.Length;
                int pieceColumns = piecesArray[0].Length;

                int[,] convertedPiece = new int[pieceRows, pieceColumns];

                for (int i = 0; i < pieceRows; i++)
                {
                    string pieceRow = piecesArray[i];

                    for (int j = 0; j < pieceColumns; j++)
                    {
                        int piecePartValue = pieceRow[j] switch
                        {
                            '.' => DOTValue,
                            'X' => XValue,
                            _ => throw new NotImplementedException(),
                        };

                        convertedPiece[i, j] = piecePartValue;
                    }
                }

                convertedPieces.Add(convertedPiece);
            }

            return convertedPieces;
        }

        /// <summary>
        /// To get better insight into the possible(coordinates) we retrieve a list of Coordinates from the board
        /// </summary>
        /// <param name="board"></param>
        private void GetAllPossiblePositions(int[,] board)
        {
            int maxX = board.GetLength(0) -1;
            int maxY = board.GetLength(1) -1;

            for (int i = 0; i <= maxX; i++)
            {
                for (int j = 0; j <= maxY; j++)
                {
                    _boardPositions.Add(new()
                    {
                        X = i,
                        Y = j,
                    });
                }
            }
        }

        /// <summary>
        /// Place all pieces on the board, after placing a piece,
        /// First check if another piece can still be placed.
        /// If no other pieces are in line, check if the current board can be considered solved
        /// </summary>
        /// <param name="piecesPlaced"></param>
        /// <param name="convertedPieces"></param>
        /// <param name="boardClone"></param>
        private void PlacePieces(ref int piecesPlaced, List<int[,]> convertedPieces, int[,] boardClone)
        {
            //Get piece to place
            int[,] pieceToPlace = convertedPieces[piecesPlaced];

            //Check every position the piece can be placed on
            List<Position> positionsToPlacePiece = CheckPositionsThatFitPiece(pieceToPlace);

            foreach (Position positionToPlacePieceOn in positionsToPlacePiece)
            {
                if (_puzzleSolved)
                    break;

                //Add the current position to the list of positions
                _currentPiecePositions.Add(positionToPlacePieceOn);

                //First, clone the board
                int[,] clonedBoard = (int[,])boardClone.Clone();

                //Place the piece on the board
                PlacePieceOnBoard(pieceToPlace, positionToPlacePieceOn, clonedBoard);
                piecesPlaced++;

                //Check if any other pieces need to be placed and place the next piece if so
                if (piecesPlaced < convertedPieces.Count)
                    PlacePieces(ref piecesPlaced, convertedPieces, clonedBoard);

                //The puzzle is considered solved if every tile is at 0
                _puzzleSolved = CheckPuzzleSolved(clonedBoard);

                if (_puzzleSolved)
                    break;

                //If the puzzle is not solved yet, removed the last saved piece position, we can always remove the last one and still keep a correct count
                //Also decrease the integer used to keep track of pieces place
                _currentPiecePositions.RemoveAt(_currentPiecePositions.Count - 1);
                piecesPlaced--;
            }
        }

        /// <summary>
        /// For every board position, check if the piece can fit on the board
        /// </summary>
        /// <param name="pieceToPlace"></param>
        /// <returns></returns>
        private List<Position> CheckPositionsThatFitPiece(int[,] pieceToPlace)
        {
            int pieceWidth = pieceToPlace.GetLength(0);
            int pieceHeight = pieceToPlace.GetLength(1);

            List<Position> positionsPieceFitsInto = new();

            foreach(Position position in _boardPositions)
            {
                int pieceMaxPositionX = pieceWidth + position.X - 1;
                int pieceMaxPositionY = pieceHeight + position.Y - 1;

                if (pieceMaxPositionX <= _maxCoordinateX &&
                    pieceMaxPositionY <= _maxCoordinateY)
                    positionsPieceFitsInto.Add(position);
            }

            return positionsPieceFitsInto;
        }

        /// <summary>
        /// Place an individual piece on the board.
        /// For every position the piece has on the board, add or subtract '1'
        /// </summary>
        /// <param name="pieceToPlace"></param>
        /// <param name="piecePosition"></param>
        /// <param name="boardClone"></param>
        private void PlacePieceOnBoard(int[,] pieceToPlace, Position piecePosition, int[,] boardClone)
        {
            int pieceWidth = pieceToPlace.GetLength(0) -1;
            int pieceHeight = pieceToPlace.GetLength(1) -1;

            //Placing the piece means to subtract 1 from every 'tile', unless it's already 0, then add 1 instead
            //Convert piece to coordinates
            List<PiecePosition> piecePositions = new();
            
            for (int i = 0; i <= pieceWidth; i++)
            { 
                for(int j = 0; j <= pieceHeight; j++)
                    piecePositions.Add(new() {
                        X = i + piecePosition.X, 
                        Y = j + piecePosition.Y,
                        TileValue = boardClone[i, j]
                    });
            }

            foreach(PiecePosition tilePosition in piecePositions)
            {
                //The tiles with a DOT (.) are not used for counting, so they can be skipped
                if (tilePosition.TileValue == DOTValue)
                    continue;

                if (boardClone[tilePosition.X, tilePosition.Y] == 0)
                    boardClone[tilePosition.X, tilePosition.Y] += 1;
                else
                    boardClone[tilePosition.X, tilePosition.Y] -= 1;
            }
        }

        /// <summary>
        /// Check if the puzzle is solved.
        /// The puzzle is solved, if every position on the board has a value of '0' 
        /// </summary>
        /// <param name="boardClone"></param>
        /// <returns></returns>
        private bool CheckPuzzleSolved(int[,] boardClone)
        {
            for(int i = 0; i < boardClone.GetLength(0) - 1; i++)
            {
                for (int j = 0;j < boardClone.GetLength(1) - 1; j++)
                {
                    if (boardClone[i, j] > 0)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Convert the correct positions of the pieces into a single string to be returned
        /// </summary>
        /// <returns></returns>
        private string CreateSolutionString()
        {
            string solutionString = string.Empty;

            foreach(Position position in _currentPiecePositions)
            {
                string positionString = $" {position.X},{position.Y}";
                solutionString += positionString;
            }

            return solutionString;
        }

        private class Position
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        private class PiecePosition : Position
        {
            public int TileValue { get; set; }
        }
    }
}
