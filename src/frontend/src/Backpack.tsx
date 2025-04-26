import { useState } from "react";
import "./Backpack.css";
import { Header } from "./Header";
import ItemDisplay from "./ItemDisplay";
import { ItemModel } from "./models/ItemModel";

export const BorderOptions = [
  "No Item Borders",
  "Show quality color borders",
  "Show marketable borders only",
] as const;
export type BorderOption = (typeof BorderOptions)[number];

export const SortOptions = [
  "Sort Backpack",
  "Sort By Quality",
  "Sort by Type",
  "Sort by Class",
  "Sort by Loadout Slot",
  "Sort by Date",
] as const;
export type SortOption = (typeof SortOptions)[number];

export const Backpack = ({
  items,
  totalBackpackSlots,
}: {
  items: ItemModel[];
  totalBackpackSlots: number;
}) => {
  const [searchTerm, setSearchTerm] = useState<string>("");

  const [borderOption, setBorderOption] = useState<BorderOption>(
    "Show quality color borders"
  );

  const [sortOption, setSortOption] = useState<SortOption>("Sort Backpack");

  return (
    <div className='backpack'>
      <Header
        searchTerm={searchTerm}
        setSearchTerm={setSearchTerm}
        borderOption={borderOption}
        setBorderOption={setBorderOption}
        sortOption={sortOption}
        setSortOption={setSortOption}
      />
      <ItemDisplay
        items={items}
        searchTerm={searchTerm.trim()}
        totalBackpackSlots={totalBackpackSlots}
      />
    </div>
  );
};
