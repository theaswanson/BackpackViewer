import clsx from "clsx";
import { useState } from "react";
import { Tooltip } from "react-tooltip";
import Item from "./Item";
import { ItemDescription } from "./ItemDescription";
import "./ItemDisplay.css";
import { ItemModel } from "./models/ItemModel";

type ItemDisplayProps = {
  items: ItemModel[];
  totalBackpackSlots: number;
};

const columnsPerPage = 10;
const rowsPerPage = 5;
const pageSize = columnsPerPage * rowsPerPage;

const PageButton = ({
  pageNumber,
  items,
  isActive,
  setCurrentPage,
}: {
  pageNumber: number;
  items: ItemModel[];
  isActive: boolean;
  setCurrentPage: (num: number) => void;
}) => {
  const itemsOnPage = itemsForPage(items, pageNumber).length;

  return (
    <button
      type='button'
      className={clsx("pageButton", {
        active: isActive,
        partial: itemsOnPage > 0 && itemsOnPage < pageSize,
        empty: itemsOnPage === 0,
      })}
      key={pageNumber}
      onClick={() => setCurrentPage(pageNumber)}
    >
      {pageNumber + 1}
    </button>
  );
};

const itemsForPage = (items: ItemModel[], page: number) =>
  items.filter(
    (i) =>
      i.backpackIndex > page * pageSize &&
      i.backpackIndex <= (page + 1) * pageSize
  );

type RenderedItem = ItemModel | null;

const getRenderedItems = (items: ItemModel[], page: number): RenderedItem[] => {
  const result: RenderedItem[] = [];

  for (let i = 1; i <= pageSize; i++) {
    const slotIndex = page * pageSize + i;
    const item = items.find((item) => item.backpackIndex === slotIndex);

    result.push(item ?? null);
  }

  return result;
};

const ItemDisplay = ({ items, totalBackpackSlots }: ItemDisplayProps) => {
  const [currentPage, setCurrentPage] = useState(0);

  const numberOfPages = Math.ceil(totalBackpackSlots / pageSize);

  const pagingButtons = Array.from(
    { length: numberOfPages },
    (_, index) => index
  );

  const displayedItems = itemsForPage(items, currentPage);

  const renderedItems = getRenderedItems(displayedItems, currentPage);

  return (
    <div className='ItemDisplay'>
      <div className='Table'>
        {renderedItems.map((item, i) => (
          <div key={i}>
            {!item ? (
              <Item />
            ) : (
              <>
                <Item id={`item-${i}`} item={item} />
                <Tooltip
                  anchorSelect={`#item-${i}`}
                  place='top'
                  style={{ zIndex: "2" }}
                  disableStyleInjection={true}
                  opacity={1}
                >
                  <ItemDescription item={item} />
                </Tooltip>
              </>
            )}
          </div>
        ))}
      </div>
      <div className='paging'>
        {pagingButtons.map((pageNumber) => (
          <PageButton
            isActive={currentPage === pageNumber}
            pageNumber={pageNumber}
            items={items}
            setCurrentPage={setCurrentPage}
            key={pageNumber}
          />
        ))}
      </div>
    </div>
  );
};

export default ItemDisplay;
