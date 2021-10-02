# DB

- `Players` table
- `Sessions` table - one per week
- `PlayerSessions` table. Historical log of who played / wanted to play every week
    - `Status`: `Selected`, `NotSelected`, `DroppedOut`, `SubbedIn`
        - No sign-up = no record for this player for this session

# "UX"

(I mean, it's a console app)

- Players managed manually in the DB

## Console app

Options:
- [X] View players
- [X] View past sessions
    - Shows the last 5 or so sessions; who played, who wanted to but wasn't selected etc.
- [X] Create new session
    - Default to the next Wednesday
- [ ] Get suggested players for session (the important bit)
    - Input which players want to play. Support copy pasting in comma separated list or whatever format exactly Polly spits out.
    - Assign each player a weighting by looking at the last 3 (arbitrary choice) sessions:
        - If they wanted to play but didn't get to (`NotSelected`), add **1**
        - If they didn't sign up, add... **0.5**..?
        - If they did play, no change
        - If they dropped out, maybe subtract 0.5? This may be a point of some controversy
        - **Crucially, these numbers can be changed!**
        - Add random noise (+/- 0.1 or so, small enough to only be relevant in case of tie) to each player to resolve tie breaks
    - Choose the 8/10/12 players with the highest weightings
    - (FRC) declare the 2 highest priority subs?
    - Option to commit this choice to DB, assigning `Selected` and `NotSelected` as relevant.
- [ ] Substitution
    - Applies to the next session that takes place in the future at time of use i.e. if it's Tuesday today, it applies to the session tomorrow.
    - Enter a player who is down to play
    - Enter the player who is going to sub in for them
    - Update/add `PlayerSessions` for these two to have `DroppedOut` and `SubbedIn` respectively