import { Children, PropsWithChildren, ReactNode, useState } from "react";
import "./Header.css";

const SelectOption = ({ children }: { children: ReactNode }) => (
  <div className='select-option'>{children}</div>
);

const ItemBordersSelect = ({ children }: PropsWithChildren) => {
  const [isOpen, setIsOpen] = useState(false);
  const selectedItem = Children.toArray(children)[0];

  return (
    <>
      <div className='select' onClick={() => setIsOpen((open) => !open)}>
        {selectedItem}
      </div>

      {isOpen ? <div>{children}</div> : undefined}
    </>
  );
};

const FilterOptions = () => (
  <ItemBordersSelect>
    <SelectOption>No Item Borders</SelectOption>
    <SelectOption>Show quality color borders</SelectOption>
    <SelectOption>Show marketable borders only</SelectOption>
  </ItemBordersSelect>
);

export const Header = () => (
  <div className='header-wrapper'>
    <div className='header'>
      <span>{">>"}</span>
      <h1>BACKPACK</h1>
    </div>
    <FilterOptions />
  </div>
);
