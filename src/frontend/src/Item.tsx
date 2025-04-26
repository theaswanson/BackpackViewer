import clsx from "clsx";
import "./Item.css";
import { ItemModel } from "./models/ItemModel";
import { Pill } from "./Pill";

interface ItemProps {
  id?: string;
  item?: ItemModel;
}

function Item({ id, item }: ItemProps) {
  return (
    <div
      id={id}
      className={clsx("Item", {
        empty: !item,
        unique: !!item,
        untradable: item && !item.tradable,
      })}
    >
      {item && (
        <>
          <img src={item.url} alt='Item icon'></img>
          {item.quantity > 1 && <Pill text={`x${item.quantity}`}></Pill>}
        </>
      )}
    </div>
  );
}

export default Item;
