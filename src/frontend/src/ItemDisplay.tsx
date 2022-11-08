import Item from "./Item";
import './ItemDisplay.css';
import { ItemModel } from "./models/ItemModel";

interface ItemDisplayProps {
  items: ItemModel[];
}

function ItemDisplay(props: ItemDisplayProps) {
  return (
    <div className='ItemDisplay'>
      {props.items.map((i) => {
        return <Item key={i.classId} item={i} />
      })}
    </div>
  );
}

export default ItemDisplay;