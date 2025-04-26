import "./Backpack.css";
import { Header } from "./Header";
import ItemDisplay from "./ItemDisplay";
import { ItemModel } from "./models/ItemModel";

export const Backpack = ({
  items,
  totalBackpackSlots,
}: {
  items: ItemModel[];
  totalBackpackSlots: number;
}) => (
  <div className='backpack'>
    <Header />
    <ItemDisplay items={items} totalBackpackSlots={totalBackpackSlots} />
  </div>
);
