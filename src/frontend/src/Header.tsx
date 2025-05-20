import {
  BorderOption,
  BorderOptions,
  SortOption,
  SortOptions,
} from "./Backpack";
import { HeaderText } from "./components/HeaderText";
import { Input } from "./components/Input";
import { CustomSelect } from "./CustomSelect";
import "./Header.css";

export const Header = ({
  searchTerm,
  setSearchTerm,
  borderOption,
  setBorderOption,
  sortOption,
  setSortOption,
}: {
  searchTerm: string;
  setSearchTerm: (value: string) => void;
  borderOption: BorderOption;
  setBorderOption: (option: BorderOption) => void;
  sortOption: SortOption;
  setSortOption: (option: SortOption) => void;
}) => (
  <div className='header-wrapper'>
    <HeaderText
      className='header'
      title='Backpack'
      titlePrefix={<span className='header-prefix'>{">>"}</span>}
    />

    <div className='options'>
      <div className='row'>
        <div className='spacer' />

        <Input
          label='Search:'
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </div>

      <div className='row'>
        <CustomSelect
          options={BorderOptions}
          selected={borderOption}
          onSelect={setBorderOption}
          className='border-options'
        />

        <CustomSelect
          options={SortOptions}
          selected={sortOption}
          onSelect={setSortOption}
          className='sort-options'
        />
      </div>
    </div>
  </div>
);
