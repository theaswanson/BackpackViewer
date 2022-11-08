import React, { useState } from 'react';
import './App.css';
import Item from './Item';
import { ItemModel } from './models/ItemModel';
import { ItemResponse } from './models/ItemResponse';
import { getItems } from './api';

function App() {
  const [checked, setChecked] = useState(false);
  const [items, setItems] = useState([] as ItemResponse[]);
  const [filteredItems, setFilteredItems] = useState([] as ItemResponse[]);

  async function loadItems() {
    const data = await getItems(checked);
    setItems(data);
    setFilteredItems(data);
  }

  function search(name: string) {
    const filteredItems = items.filter((i) => i.name.toLowerCase().includes(name.toLowerCase()))
    setFilteredItems(filteredItems);
  }

  return (
    <div className="App">
      <header className="App-header">
        <div>
          <label htmlFor="useMockResponse">Use mock response</label>
          <input
            type="checkbox"
            id="useMockResponse"
            checked={checked}
            onChange={(e) => setChecked(e.target.checked)}
          />
        </div>

        <button onClick={loadItems}>Get Items</button>

        {items && items.length > 0 &&
          <input onChange={(e) => { search(e.target.value)}} />}

        {filteredItems &&
          <>
            <div className='items'>
              {filteredItems.map((i) => {
                const item = {
                  name: i.name,
                  quantity: i.quantity,
                  url: i.iconUrl,
                  tradable: i.tradable ?? true
                } as ItemModel;
                return <Item
                  key={i.classId}
                  item={item}
                />
              })}
            </div>
          </>
        }
      </header>
    </div>
  );
}

export default App;
