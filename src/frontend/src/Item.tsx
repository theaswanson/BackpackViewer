import React from 'react';
import { ItemModel } from './models/ItemModel';

interface ItemProps {
  item: ItemModel
}

function Item(props: ItemProps) {
  return (
    <div>
      <h1>{props.item.name} ({props.item.quantity})</h1>
      <img src={props.item.url} alt="Item icon"></img>
    </div>
  );
}

export default Item;
