import clsx, { ClassValue } from "clsx";
import { BorderOption } from "./Backpack";
import { Pill } from "./components/Pill";
import "./Item.css";
import { ItemModel } from "./models/ItemModel";
import { ItemQuality } from "./models/ItemQuality";

interface ItemProps {
  id?: string;
  item?: ItemModel;
  borderOption: BorderOption;
}

const getClassValue = (
  item: ItemModel | undefined,
  borderOption: BorderOption
): ClassValue => {
  switch (borderOption) {
    // TODO: handle case for Show marketable borders only
    case "Show marketable borders only":
    case "Show quality color borders":
      return { empty: !item, untradable: item && !item.tradable };

    case "No Item Borders":
    default:
      return "empty";
  }
};

function Item({ id, item, borderOption }: ItemProps) {
  return (
    <div
      id={id}
      className={clsx(
        "Item",
        getClassValue(item, borderOption),
        item && ItemQuality[item.quality].toLowerCase()
      )}
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
