import clsx from "clsx";
import "./Item.css";
import { ItemModel } from "./models/ItemModel";
import { Pill } from "./Pill";

interface ItemProps {
  item: ItemModel;
}

function Item(props: ItemProps) {
  return (
    <div
      className={clsx("Item", {
        untradable: !props.item.tradable,
      })}
    >
      <img src={props.item.url} alt='Item icon'></img>
      {props.item.quantity > 1 && (
        <Pill text={`x${props.item.quantity}`}></Pill>
      )}
    </div>
  );
}

export default Item;
