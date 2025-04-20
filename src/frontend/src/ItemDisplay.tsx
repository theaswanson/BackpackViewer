import clsx from "clsx";
import { useState } from "react";
import Item from "./Item";
import "./ItemDisplay.css";
import { ItemModel } from "./models/ItemModel";

type ItemDisplayProps = {
  items: ItemModel[];
};

const columnsPerPage = 10;
const rowsPerPage = 5;
const pageSize = columnsPerPage * rowsPerPage;

const numberOfItemsOnPage = (pageIndex: number, totalItemCount: number) =>
  Math.min(pageSize, totalItemCount - pageIndex * pageSize);

const PageButton = ({
  pageNumber,
  totalItemCount,
  setCurrentPage,
}: {
  pageNumber: number;
  totalItemCount: number;
  setCurrentPage: (num: number) => void;
}) => {
  const itemsOnPage = numberOfItemsOnPage(pageNumber, totalItemCount);

  return (
    <button
      type='button'
      className={clsx("pageButton", {
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

const ItemDisplay = ({ items }: ItemDisplayProps) => {
  const [currentPage, setCurrentPage] = useState(0);

  const numberOfPages = Math.ceil(items.length / pageSize);

  const pagingButtons = Array.from(
    { length: numberOfPages },
    (_, index) => index
  );

  const displayedItems = items.slice(
    currentPage * pageSize,
    (currentPage + 1) * pageSize
  );

  return (
    <div className='ItemDisplay'>
      <div className='Table'>
        {displayedItems.map((i) => (
          <Item key={i.classId} item={i} />
        ))}
      </div>
      <div className='paging'>
        {pagingButtons.map((pageNumber) => (
          <PageButton
            pageNumber={pageNumber}
            totalItemCount={items.length}
            setCurrentPage={setCurrentPage}
          />
        ))}
      </div>
    </div>
  );
};

export default ItemDisplay;
