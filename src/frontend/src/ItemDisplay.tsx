import clsx from "clsx";
import { useState } from "react";
import { Tooltip } from "react-tooltip";
import { BorderOption } from "./Backpack";
import Item from "./Item";
import { ItemDescription } from "./ItemDescription";
import "./ItemDisplay.css";
import { ItemModel } from "./models/ItemModel";

type ItemDisplayProps = {
  items: ItemModel[];
  totalBackpackSlots: number;
  searchTerm: string;
  borderOption: BorderOption;
};

const columnsPerPage = 10;
const rowsPerPage = 5;
const pageSize = columnsPerPage * rowsPerPage;

const PageButton = ({
  pageNumber,
  items,
  isActive,
  isFiltered,
  setCurrentPage,
}: {
  pageNumber: number;
  items: ItemModel[];
  isActive: boolean;
  isFiltered: boolean;
  setCurrentPage: (num: number) => void;
}) => {
  const itemsOnPage = itemsForPage(items, pageNumber, isFiltered).length;

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

const itemsForPage = (items: ItemModel[], page: number, isFiltered: boolean) =>
  /**
   * If the backpack is filtered, then we don't want to display the items according to their
   * backpack slot.
   */
  isFiltered
    ? items.slice(page * pageSize, (page + 1) * pageSize)
    : items.filter(
        (i) =>
          i.backpackIndex > page * pageSize &&
          i.backpackIndex <= (page + 1) * pageSize
      );

type RenderedItem = ItemModel | null;

const getRenderedItems = (
  items: ItemModel[],
  page: number,
  isFiltered: boolean
): RenderedItem[] => {
  if (isFiltered) {
    if (items.length == pageSize) {
      return items;
    }

    const emptyItemsCount = pageSize - items.length;

    return [...items, ...Array.from({ length: emptyItemsCount }, () => null)];
  }

  const result: RenderedItem[] = [];

  for (let i = 1; i <= pageSize; i++) {
    const slotIndex = page * pageSize + i;
    const item = items.find((item) => item.backpackIndex === slotIndex);

    result.push(item ?? null);
  }

  return result;
};

const ItemDisplay = ({
  items,
  searchTerm,
  totalBackpackSlots,
  borderOption,
}: ItemDisplayProps) => {
  const [currentPage, setCurrentPage] = useState(0);

  const numberOfPages = Math.ceil(totalBackpackSlots / pageSize);

  const pagingButtons = Array.from(
    { length: numberOfPages },
    (_, index) => index
  );

  const isFiltered = searchTerm.length > 0;

  const filteredItems = isFiltered
    ? items.filter(
        (item) =>
          item.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
          item.customName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
          item.customDescription
            ?.toLowerCase()
            .includes(searchTerm.toLowerCase())
      )
    : items;

  const displayedItems = itemsForPage(filteredItems, currentPage, isFiltered);

  const renderedItems = getRenderedItems(
    displayedItems,
    currentPage,
    isFiltered
  );

  return (
    <div className='ItemDisplay'>
      <div className='Table'>
        {renderedItems.map((item, i) => (
          <div key={i}>
            {!item ? (
              <Item borderOption={borderOption} />
            ) : (
              <>
                <Item
                  id={`item-${i}`}
                  item={item}
                  borderOption={borderOption}
                />
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
            items={filteredItems}
            isFiltered={isFiltered}
            setCurrentPage={setCurrentPage}
            key={pageNumber}
          />
        ))}
      </div>
    </div>
  );
};

export default ItemDisplay;
