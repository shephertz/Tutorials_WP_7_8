Tutorials_WP_7_8
================

Tutorials on building Windows Phone 7 and 8 apps using App42 API and AppWarp.

SETUP OF PROJECT FOR WP7 AND WP8:

1. Download Project and Unblock Zipped file issue

a. If downloaded files are in zip format we need to unblock it (By right clicking on zipped file and then clicking on property we will get Unblock checkbox)
   before Unzipping it.


2. App HQ sign up ( Join with your account or sign up for new account)

3. App HQ create app (appwarp) - by clicking on Create App button. You will get  API_KEY and SECRET_KEY. Provide API_KEY and SECRET_KEY obtained in GlobalContext.cs file
Note: For tic tac toe specify max user as 2

4. App HQ create room and get its room id. Provide Room ID obtained in GlobalContext.cs file
   
   
   
5.
a. To run in emulator just Run the project or click on debug in IDE . We Can run 2 instances on a single machine
   or different machines.
b. To Run it on Window 7 phone use TicTacToeAppWarp.xap file with Zune to Install game . File will be obtained after building solution in following folder- 
  (Tutorials_WP_7_8\TicTacToeAppWarp\Bin\Debug)


6. How to play the game.

a. Once one user Join the game by entering his name and clicking on join it will wait till secvond user join
b. Once Second user will join first user will get message for his move and second player will have to wait till first player move and game proceed with alternate
   moves
c. User clicking on new gtame button before game completion will lose game and new game will be initialized.
d. User clicking on end game button before game completion will lose game and other user will get notification of it











 
 
