import { useState } from "react";
import "./App.css";
import { ItemModel } from "./models/ItemModel";
import { ItemResponse } from "./models/ItemResponse";
import { getItems } from "./api";
import ItemDisplay from "./ItemDisplay";

function App() {
  const [loading, setLoading] = useState(false);
  const [useMockResponse, setUseMockResponse] = useState(false);
  const [items, setItems] = useState([] as ItemResponse[]);
  const [filteredItems, setFilteredItems] = useState([] as ItemResponse[]);

  async function loadItems() {
    setLoading(true);
    try {
      const data = await getItems(useMockResponse);
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
    <div className="App">
      <header className="App-header">
        <div>
          <label htmlFor="useMockResponse">Use mock response</label>
          <input
            type="checkbox"
            id="useMockResponse"
            checked={useMockResponse}
            onChange={(e) => setUseMockResponse(e.target.checked)}
          />
        </div>

        <button onClick={loadItems} disabled={loading}>
          Get Items
        </button>

        {items && items.length > 0 && (
          <input
            onChange={(e) => {
              search(e.target.value);
            }}
          />
        )}

        <ItemDisplay
          items={filteredItems.map((i) => {
            return {
              classId: i.classId,
              name: i.name,
              quantity: i.quantity,
              url: i.iconUrl,
              tradable: i.tradable ?? true,
            } as ItemModel;
          })}
        />
      </header>
    </div>
  );
}

export default App;
