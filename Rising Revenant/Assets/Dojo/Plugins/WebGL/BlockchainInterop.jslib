mergeInto(LibraryManager.library, {
  FetchCurrentBlockNumber: function () {
    // Convert Emscripten string pointer to JavaScript string
    var url = "http://localhost:5050"; // Your StarkNet node endpoint
    var data = JSON.stringify({
      "jsonrpc": "2.0",
      "method": "starknet_getBlockByNumber",
      "params": ["latest", false],
      "id": 1
    });

    // Asynchronously send the request to the StarkNet node
    fetch(url, {
      method: 'POST',
      headers: {'Content-Type': 'application/json'},
      body: data,
    })
    .then(response => response.json())
    .then(data => {
      console.log("Block Number: ", data.result.block_number);
      // Use SendMessage to send the block number back to a Unity GameObject
      // Replace "YourGameObjectName" with the name of your GameObject
      // Replace "OnBlockNumberReceived" with the name of the method in your C# script
      console.log(blockNumber);
      var blockNumber = data.result.block_number.toString();
      _SendMessage("InitEntities", "OnBlockNumberReceived", blockNumber);
    })
    .catch((error) => {
      console.error('Error:', error);
    });
  }
});