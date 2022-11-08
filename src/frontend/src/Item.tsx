import { ItemModel } from './models/ItemModel';
import './Item.css'
import clsx from 'clsx';

interface ItemProps {
  item: ItemModel
}

function Item(props: ItemProps) {
  return (
    <div className={clsx("Item", {
      untradable: !props.item.tradable
    })}>
      <h1>{props.item.name}</h1>
      <img
        src={props.item.url}
        width={200}
        height={200}
        alt="Item icon"></img>
      {props.item.quantity > 1 && <p>x{props.item.quantity}</p>}
    </div>
  );
}

export default Item;
