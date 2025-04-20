import { ReactNode, useState } from "react";
import { Chevron } from "./Chevron";

const SelectOption = ({ children }: { children: ReactNode }) => (
  <div className='select-option'>{children}</div>
);

export const CustomSelect = ({ options }: { options: string[] }) => {
  const [isOpen, setIsOpen] = useState(false);
  const selectedItem = options[0];

  return (
    <div className='select'>
      <div
        className='select-option main'
        onClick={() => setIsOpen((open) => !open)}
      >
        {selectedItem} <Chevron />
      </div>

      {isOpen ? (
        <div className='dropdown'>
          {options.map((option) => (
            <SelectOption>{option}</SelectOption>
          ))}
        </div>
      ) : undefined}
    </div>
  );
};
