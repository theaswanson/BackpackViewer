import { CustomSelect } from "./CustomSelect";
import "./Header.css";

const ViewOptions = () => (
  <>
    <CustomSelect
      options={[
        "No Item Borders",
        "Show quality color borders",
        "Show marketable borders only",
      ]}
    />
    <CustomSelect
      options={[
        "Sort Backpack",
        "Sort By Quality",
        "Sort by Type",
        "Sort by Class",
        "Sort by Loadout Slot",
        "Sort by Date",
      ]}
    />
  </>
);

export const Header = () => (
  <div className='header-wrapper'>
    <div className='header'>
      <span>{">>"}</span>
      <h1>BACKPACK</h1>
    </div>
    <ViewOptions />
  </div>
);
