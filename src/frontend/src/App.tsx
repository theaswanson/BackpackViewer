import React, { useEffect, useState } from 'react';
import './App.css';
import Item from './Item';
import { ItemModel } from './models/ItemModel';
import { ItemResponse } from './models/ItemResponse';
import { getItems } from './api';

function App() {
  const [items, setItems] = useState([] as ItemResponse[]);

  useEffect(() => {
    async function fetchData() {
      const data = await getItems();
      setItems(data);
    }
    fetchData();
  }, []);

  return (
    <div className="App">
      <header className="App-header">
        {items && items.map((i) => {
          return <Item key={i.classId} item={{name: i.name, quantity: i.quantity, url: i.iconUrl} as ItemModel} />
        })}
      </header>
    </div>
  );
}

export default App;
