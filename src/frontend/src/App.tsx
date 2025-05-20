import { useState } from "react";
import { getItems } from "./api";
import { Backpack } from "./Backpack";
import { ItemModel } from "./models/ItemModel";
import { ItemResponse } from "./models/ItemResponse";

import "./App.css";
import { Checkbox } from "./components/Checkbox";
import { HeaderText } from "./components/HeaderText";
import { Input } from "./components/Input";
import { ItemQuality } from "./models/ItemQuality";

const getQualityName = (quality: ItemQuality) => {
  switch (quality) {
    case ItemQuality.SelfMade:
      return "Self-Made";
    case ItemQuality.Collectors:
      return "Collector's";
    default:
      return ItemQuality[quality];
  }
};

const getItemDisplayName = (item: ItemResponse): string => {
  switch (item.quality) {
    case ItemQuality.Genuine:
    case ItemQuality.Vintage:
    case ItemQuality.Unusual:
    case ItemQuality.Community:
    case ItemQuality.Valve:
    case ItemQuality.SelfMade:
    case ItemQuality.Strange:
    case ItemQuality.Haunted:
    case ItemQuality.Collectors:
      return `${getQualityName(item.quality)} ${item.name}`;

    default:
      return item.name;
  }
};

function App() {
  const [loading, setLoading] = useState(false);
  const [useMockResponse, setUseMockResponse] = useState(false);
  const [steamId, setSteamId] = useState<string>("");
  const [items, setItems] = useState<ItemResponse[]>([]);
  const [totalBackpackSlots, setTotalBackpackSlots] = useState(50);

  const backpackItems: ItemModel[] = items.map((i) => ({
    id: i.id,
    name: i.name,
    displayName: getItemDisplayName(i),
    customName: i.customName === null ? undefined : i.customName,
    description: i.description,
    customDescription:
      i.customDescription === null ? undefined : i.customDescription,
    quantity: i.quantity,
    url: i.iconUrl,
    tradable: i.tradable,
    level: i.level,
    type: i.type,
    uses: i.uses,
    backpackIndex: i.backpackIndex,
    quality: i.quality,
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
      setTotalBackpackSlots(response.totalBackpackSlots);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className='App'>
      <header className='App-header'>
        <div style={{ display: "flex", flexDirection: "column", gap: "1rem" }}>
          <HeaderText title='Settings' />

          <div
            style={{
              display: "flex",
              flexDirection: "row",
              alignItems: "flex-end",
              justifyContent: "center",
              gap: "2rem",
              flexWrap: "wrap",
            }}
          >
            <Input
              id='steamId'
              type='number'
              min={0}
              label='Steam ID'
              value={steamId}
              onChange={(e) => setSteamId(e.target.value)}
            />

            <Checkbox
              label='Use mock response'
              isChecked={useMockResponse}
              onChange={(e) => setUseMockResponse(e.target.checked)}
            />
          </div>

          <button onClick={loadItems} disabled={loading}>
            Get Items
          </button>
        </div>
      </header>

      <Backpack items={backpackItems} totalBackpackSlots={totalBackpackSlots} />
    </div>
  );
}

export default App;
