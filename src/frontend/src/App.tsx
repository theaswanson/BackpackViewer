import { useState } from "react";
import { getItems } from "./api";
import "./App.css";
import { Backpack } from "./Backpack";
import { ItemModel } from "./models/ItemModel";
import { ItemResponse } from "./models/ItemResponse";

function App() {
  const [loading, setLoading] = useState(false);
  const [useMockResponse, setUseMockResponse] = useState(false);
  const [steamId, setSteamId] = useState<string>();
  const [items, setItems] = useState([] as ItemResponse[]);
  const [filteredItems, setFilteredItems] = useState([] as ItemResponse[]);

  const backpackItems: ItemModel[] = filteredItems.map((i) => ({
    classId: i.classId,
    name: i.name,
    quantity: i.quantity,
    url: i.iconUrl,
    tradable: i.tradable ?? true,
  }));

  async function loadItems() {
    if (!steamId) {
      return;
    }

    setLoading(true);
    try {
      const data = await getItems(steamId, useMockResponse);
      setItems(data);
      setFilteredItems(data);
    } finally {
      setLoading(false);
    }
  }

  function search(name: string) {
    const filteredItems = items.filter((i) =>
      i.name.toLowerCase().includes(name.toLowerCase())
    );
    setFilteredItems(filteredItems);
  }

  return (
    <div className='App'>
      <header className='App-header'>
        <div style={{ display: "flex", flexDirection: "column", gap: "1rem" }}>
          <div>
            <label htmlFor='steamId'>Steam ID</label>
            <input
              id='steamId'
              type='number'
              value={steamId}
              onChange={(e) => setSteamId(e.target.value)}
            />
          </div>
          <div>
            <label htmlFor='useMockResponse'>Use mock response</label>
            <input
              type='checkbox'
              id='useMockResponse'
              checked={useMockResponse}
              onChange={(e) => setUseMockResponse(e.target.checked)}
            />
          </div>
          <button onClick={loadItems} disabled={loading}>
            Get Items
          </button>
          {items && items.length > 0 && (
            <input
              placeholder='Search...'
              onChange={(e) => {
                search(e.target.value);
              }}
            />
          )}
        </div>
      </header>
      <Backpack items={backpackItems} />
    </div>
  );
}

export default App;
