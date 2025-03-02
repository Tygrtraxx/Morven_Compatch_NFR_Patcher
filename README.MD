# Morven Compatch: No Fervour Rebalance (NFR) Patcher

**Morven Compatch NFR Patcher** is a mod for [Crusader Kings III](https://store.steampowered.com/app/1158310/Crusader_Kings_III/) designed to disable a problematic mod from [Kaepbora's Morven's Mods Compatch](https://steamcommunity.com/sharedfiles/filedetails/?id=3001489429). The original compatch has a mod with a "religious mania" mechanic—and other minor features intended to curb runaway growth—actually triggers excessive heresy, causing mass destabilization and penalizing large, successful religions. This patcher removes the associated files so that thriving religions can be properly rewarded.

## Prerequisites

- **Kaepbora's Morven's Mods 1.14 Compatch:**  
  You must have [Kaepbora's Morven's Mods 1.14 Compatch](https://steamcommunity.com/sharedfiles/filedetails/?id=3001489429) installed before using this patcher.

## How It Works

Rather than modifying the original mod’s files directly (which can be overwritten by updates), this patcher:
- **Copies the mod files:** Creates a duplicate in your mods folder.
- **Applies targeted modifications:** Removes only the specific files associated with [Fervour Midpoint Rebalance – Fervor Tends Towards 50%](https://steamcommunity.com/sharedfiles/filedetails/?id=2249720608) that cause the imbalance.
- **Keeps the original intact:** Allowing you to reinstall or re-enable the unmodified mod if needed.

## Installation & Usage

1. **Download the Patch:**  
   Get the latest release of the Morven Compatch NFR Patcher from our [GitHub Releases](https://github.com/Tygrtraxx/Morven_Compatch_NFR_Patcher/releases).

2. **Run the Patcher:**  
   Launch the patcher. When it starts, you’ll be prompted to specify the locations of your Steam installation folder and your Crusader Kings III mods folder (usually located at:  
   `Documents/Paradox Interactive/Crusader Kings III`).

3. **Patch Your Game:**  
   After setting the folder paths, click the **Patch** button. The patcher will:
   - Copy the necessary mod files.
   - Apply the fixes to disable the unbalanced heresy triggers.
   - Remove the files associated with [Fervour Midpoint Rebalance – Fervor Tends Towards 50%](https://steamcommunity.com/sharedfiles/filedetails/?id=2249720608).

4. **Disable Conflicting Mods:**  
   **Important:** Before launching the game, make sure you disable or uninstall both of the following mods:
   - [Morven's Mods Compatch](https://steamcommunity.com/sharedfiles/filedetails/?id=3001489429).
   - [Fervour Midpoint Rebalance – Fervor Tends Towards 50%](https://steamcommunity.com/sharedfiles/filedetails/?id=2249720608).  
   This step is crucial to avoid mod conflicts, corrupted saves, or crashes.

5. **Enjoy:**  
   Launch Crusader Kings III.
 
**Multiplayer Note:**  
For anyone playing in multiplayer with this patcher, it is **HIGHLY RECOMMENDED** that all players run this patcher again before starting a playthrough. Since this is considered a local mod after patching, it does not update automatically when the original Kaepbora's Compatch is updated. For example, if Player 1 patched their mod two weeks ago while Player 2 patched theirs more recently—and Kaepbora has since tweaked his mod—the players will have different mod checksums. This mismatch makes it impossible to play together.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
