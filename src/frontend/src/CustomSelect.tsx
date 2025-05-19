import { CSSProperties, HTMLAttributes, ReactNode, useState } from "react";
import { Chevron } from "./Chevron";
import "./CustomSelect.css";

const SelectOption = ({
  children,
  ...rest
}: { children: ReactNode } & HTMLAttributes<HTMLDivElement>) => (
  <div {...rest} className='select-option'>
    {children}
  </div>
);

interface IProps<T> {
  options: readonly T[];
  selected?: T;
  onSelect: (option: T) => void;
  className?: string;
  style?: CSSProperties;
}

export const CustomSelect = <T extends string>({
  options,
  selected,
  onSelect,
  className,
  style,
}: IProps<T>) => {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <div className={className ? `${className} select` : "select"} style={style}>
      <div
        className='select-option main'
        onClick={() => setIsOpen((open) => !open)}
      >
        {selected} <Chevron />
      </div>

      {isOpen ? (
        <div className='dropdown'>
          {options.map((option) => (
            <SelectOption
              onClick={() => {
                setIsOpen(false);
                onSelect(option);
              }}
              key={option}
            >
              {option}
            </SelectOption>
          ))}
        </div>
      ) : undefined}
    </div>
  );
};
