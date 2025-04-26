import { ChangeEventHandler } from "react";
import "./Search.css";

export const Search = ({
  searchTerm,
  onChange,
}: {
  searchTerm: string;
  onChange: ChangeEventHandler<HTMLInputElement>;
}) => (
  <div className='search'>
    <h3>Search:</h3>
    <input value={searchTerm} onChange={onChange} />
  </div>
);
