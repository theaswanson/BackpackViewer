import "./ItemDescription.css";

const LimitedUse = ({ uses }: { uses: number }) => (
  <p className='limited-use'>This is a limited use item. Uses: {uses}</p>
);

export const ItemDescription = () => (
  <div className='item-desc'>
    <div className='title'>
      <h1>Name Tag</h1>
      <h2>Level 1 Tool</h2>
      <p>Changes the name of an item in your backpack</p>
    </div>
    <LimitedUse uses={1} />
    <p>( Not Tradable or Marketable )</p>
  </div>
);
