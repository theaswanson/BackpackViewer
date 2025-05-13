import {
  BorderOption,
  BorderOptions,
  SortOption,
  SortOptions,
} from "./Backpack";
import { CustomSelect } from "./CustomSelect";
import "./Header.css";
import { Search } from "./Search";

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
    <div className='header'>
      <span>{">>"}</span>
      <h1>BACKPACK</h1>
    </div>

    <div className='options'>
      <div className='row'>
        <div style={{ width: "565px" }} />

        <Search
          searchTerm={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </div>

      <div className='row'>
        <CustomSelect
          options={BorderOptions}
          selected={borderOption}
          onSelect={setBorderOption}
          style={{ width: "565px" }}
        />

        <CustomSelect
          options={SortOptions}
          selected={sortOption}
          onSelect={setSortOption}
          style={{ width: "400px" }}
        />
      </div>
    </div>
  </div>
);
