import { ChangeEventHandler } from "react";
import "./Checkbox.css";

export const Checkbox = ({
  label,
  isChecked,
  onChange,
}: {
  label: string;
  isChecked: boolean;
  onChange: ChangeEventHandler<HTMLInputElement>;
}) => (
  <div className='checkbox-group'>
    <label>
      <input
        type='checkbox'
        checked={isChecked}
        onChange={onChange}
        className={isChecked ? "checked" : ""}
      />
      <span>{label}</span>
    </label>
  </div>
);
