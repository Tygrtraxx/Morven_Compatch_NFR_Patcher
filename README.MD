# Morven Compatch: No Fervour Rebalance (NFR) Patcher

**Morven Compatch NFR Patcher** is a mod for [Crusader Kings III](https://store.steampowered.com/app/1158310/Crusader_Kings_III/) designed to disable a problematic mod from [Kaepbora's Morven's Mods Compatch](https://steamcommunity.com/sharedfiles/filedetails/?id=3001489429). The original mod’s "religious mania" mechanic—and other minor features intended to curb runaway growth—actually triggers excessive heresy, causing mass destabilization and penalizing large, successful religions. This patcher removes the associated files so that thriving religions can be properly rewarded.

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

2. **Configure Folders:**  
   - **Steam Folder:** Point to your Steam (or SteamApps) installation folder.
   - **Mods Folder:** Specify your mods folder, usually located in:  
     `Documents/Paradox Interactive/Crusader Kings III`

3. **Run the Patcher:**  
   Launch the patcher and click the **Patch** button. The tool will:
   - Copy the necessary mod files.
   - Apply the fixes to disable the unbalanced heresy triggers.
   - Remove the files associated with [Fervour Midpoint Rebalance – Fervor Tends Towards 50%](https://steamcommunity.com/sharedfiles/filedetails/?id=2249720608).

4. **Update Your Mods:**  
   - **Disable or uninstall both** the following:  
     - The original [Morven's Mods Compatch](https://steamcommunity.com/sharedfiles/filedetails/?id=3001489429).  
     - [Fervour Midpoint Rebalance – Fervor Tends Towards 50%](https://steamcommunity.com/sharedfiles/filedetails/?id=2249720608) (its files are removed by the patcher).  
   - **Important:** Failing to disable or uninstall both mods can lead to conflicts, corrupted saves, or even crashes.  
   - Enable the newly patched version in your mod manager.

5. **Enjoy:**  
   Start Crusader Kings III and enjoy a more balanced gameplay experience!

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
