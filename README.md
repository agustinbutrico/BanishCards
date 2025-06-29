# BanishCards Mod for Rogue Tower

## Take control of your upgrade pool.

### BanishCards allows you to remove up to 3 unwanted upgrade cards during a Rogue Tower run, reducing RNG frustration and letting you execute clean, focused strategies for challenge runs and personal builds.
Features

    Banish Upgrades:
    Select and banish up to 3 upgrade cards per run instead of accepting them.

    Strategic Pruning:
    Banishing a card will:

        Permanently remove it from your upgrade pool for the current run.

        Prevent any upgrades that require it as a prerequisite from appearing later.

        Not provide any benefits—the card is simply removed.

    Keep Your Build Clean:
    Perfect for achievement runs like “Win One-Lane using only Ballistas,” enabling you to remove upgrades that would otherwise dilute your intended strategy.

    Balanced:
    A strict 3-card banish limit per run keeps the feature strategic rather than exploitable.

## Why?

### Rogue Tower’s upgrades are essential for scaling difficulty, but they can lead to RNG-heavy runs when pursuing challenge achievements or specialized builds. By enabling selective removal, BanishCards adds meaningful choices without disrupting the game’s balance.
Installation

    Install BepInEx for Rogue Tower if you have not already.

    Download the BanishCards.dll and place it in your BepInEx/plugins folder.

    Launch the game and enjoy controlled upgrade management.

## Planned Features

    UI highlighting to indicate Banishable cards.

    Configurable banish limit via config file.

    Optional toggle for enabling/disabling banish mode mid-run.

## Relevant Classes You Will Work With

    UpgradeCard (@02000068) – represents the upgrade cards you draw during the run.

    CardManager (@0200000E) – likely manages available and drawn cards.

    GameManager (@0200001B) – central game logic, often where run-level states can be stored (e.g., banishCount).

    TowerUpgradeCard, DOTUpgradeCard, GoldRushCard, etc. – subclasses or variations of upgrade cards.

    UpgradeButton (@02000067) – likely tied to the UI element you click to accept an upgrade.

    UpgradeManager (@02000069) – may handle upgrade granting logic.

## Where BanishCards mod will hook

Your mod’s purpose:

    Allow the player to banish a card (remove it from the pool permanently during the run).

### Based on reverse engineering, the cleanest insertion points are:

✅ CardManager:

    It holds availableCards (the pool you want to modify).

    It shows cards in DrawCards.

    Removes cards after activation.

    Your mod would add a UI or hotkey to:

        Show cards on-screen during selection.

        On right-click or button press, remove the selected card permanently from availableCards.

✅ UpgradeManager or UpgradeButton:

    Only if you want to add a paid banish (costs XP or card count).

    Otherwise, you don’t need to touch them.

✅ Optional future hook:

    If you want a UI showing “Banished Cards” during the run, you will need a List<UpgradeCard> to store banished cards and display them via a small overlay.
