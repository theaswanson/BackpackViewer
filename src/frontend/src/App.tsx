import { useState } from "react";
import { getItems } from "./api";
import { Backpack } from "./Backpack";
import { ItemModel } from "./models/ItemModel";
import { ItemResponse } from "./models/ItemResponse";

import "./App.css";

function App() {
  const [loading, setLoading] = useState(false);
  const [useMockResponse, setUseMockResponse] = useState(false);
  const [steamId, setSteamId] = useState<string>("");
  const [searchTerm, setSearchTerm] = useState<string>("");
  const [items, setItems] = useState<ItemResponse[]>([]);
  const [totalBackpackSlots, setTotalBackpackSlots] = useState(50);

  const [filteredItems, setFilteredItems] = useState<ItemResponse[]>([]);

  const backpackItems: ItemModel[] = filteredItems.map((i) => ({
    id: i.id,
    name: i.name,
    description: i.description,
    quantity: i.quantity,
    url: i.iconUrl,
    tradable: i.tradable,
    level: i.level,
    type: i.type,
    uses: i.uses,
    backpackIndex: i.backpackIndex,
  }));

  async function loadItems() {
    if (!steamId) {
      return;
    }

    setLoading(true);
    try {
      const response = await getItems(steamId, useMockResponse);
      const items = response.items.sort(
        (a, b) => a.backpackIndex - b.backpackIndex
      );
      setItems(items);
      setFilteredItems(items);
      setTotalBackpackSlots(response.totalBackpackSlots);
    } finally {
      setLoading(false);
    }
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
              value={searchTerm}
              onChange={(e) => {
                setSearchTerm(e.target.value);
              }}
            />
          )}
        </div>
      </header>
      <Backpack
        items={backpackItems}
        searchTerm={searchTerm.trim()}
        totalBackpackSlots={totalBackpackSlots}
      />
    </div>
  );
}

export default App;
