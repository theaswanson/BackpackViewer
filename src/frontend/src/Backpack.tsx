import "./Backpack.css";
import { Header } from "./Header";
import ItemDisplay from "./ItemDisplay";
import { ItemModel } from "./models/ItemModel";

export const Backpack = ({
  items,
  totalBackpackSlots,
  searchTerm,
}: {
  items: ItemModel[];
  totalBackpackSlots: number;
  searchTerm: string;
}) => (
  <div className='backpack'>
    <Header />
    <ItemDisplay
      items={items}
      searchTerm={searchTerm}
      totalBackpackSlots={totalBackpackSlots}
    />
  </div>
);
