import "./ItemDescription.css";
import { ItemModel } from "./models/ItemModel";

const LimitedUse = ({ uses }: { uses: number }) => (
  <p className='limited-use'>This is a limited use item. Uses: {uses}</p>
);

export const ItemDescription = ({ item }: { item: ItemModel }) => (
  <div className='item-desc'>
    <div className='title'>
      <h1>{item.name}</h1>
      {item.level && (
        <h2>
          Level {item.level} {item.type}
        </h2>
      )}
      {item.description && item.description.length > 0 && (
        <p>{item.description}</p>
      )}
    </div>
    {item.uses && <LimitedUse uses={item.uses} />}
    {!item.tradable && <p>( Not Tradable or Marketable )</p>}
  </div>
);
