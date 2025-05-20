import { InputHTMLAttributes } from "react";
import "./Input.css";

type IProps = {
  /**
   * An optional label to go with the input. If specified, `id` should also be specified.
   */
  label?: string;
} & InputHTMLAttributes<HTMLInputElement>;

export const Input = ({ label, id, ...props }: IProps) => (
  <div className='input-group'>
    {label && <label htmlFor={id}>{label}</label>}
    <input id={id} {...props} className='input' />
  </div>
);
