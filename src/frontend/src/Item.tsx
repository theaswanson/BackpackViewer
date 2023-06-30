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
      <img
        src={props.item.url}
        width={150}
        height={150}
        alt="Item icon"></img>
      {props.item.quantity > 1 && <p>x{props.item.quantity}</p>}
    </div>
  );
}

export default Item;
