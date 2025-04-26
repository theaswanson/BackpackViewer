import {
  BorderOption,
  BorderOptions,
  SortOption,
  SortOptions,
} from "./Backpack";
import { CustomSelect } from "./CustomSelect";
import "./Header.css";

export const Header = ({
  borderOption,
  setBorderOption,
  sortOption,
  setSortOption,
}: {
  borderOption: BorderOption;
  setBorderOption: (option: BorderOption) => void;
  sortOption: SortOption;
  setSortOption: (option: SortOption) => void;
}) => (
  <div className='header-wrapper'>
    <div className='header'>
      <span>{">>"}</span>
      <h1>BACKPACK</h1>
    </div>

    <CustomSelect
      options={BorderOptions}
      selected={borderOption}
      onSelect={setBorderOption}
    />

    <CustomSelect
      options={SortOptions}
      selected={sortOption}
      onSelect={setSortOption}
    />
  </div>
);
