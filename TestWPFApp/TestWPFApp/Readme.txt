This Solution is a 'solution' to an assignment required in a previous interview.

For this assignment, it was required to create an algorithm (in C#) to solve several puzzles with identical structure and constraints.
However, I was unable to really think of any particular algorithm, or logic, behind this puzzle.

Considering I could not really solve this puzzle in any formulaic way, I instead opted to use brute force instead.
I did this by creating a application that went through every possible combination to place each piece on the board.
Every time, after the last piece was placed on the board, I then check if the puzzle was solved,
This is simply checking if each tile was at 0

The pieces always have a value of 1 or 0 for each tile,
Using this method, I believed I also eliminated the need to implement any particular code for the depth-setting.
Since the application ran through every possibility, it ultimately would not matter if the board piece was at 0, 1, 2 or any other value.
It would have kept adding tiles as needed.

Unfortunately, I only really managed to solve the first puzzle.
The ones after that all seemed to fail, but even worse, I had clearly underestimated, or failed to account for, how many possible combinations really existed.
This meant that even the second puzzle already took several minutes to process.
This did lead me to the conclusion that even if I managed to fully make it work.
It could hardly be called a proper solution