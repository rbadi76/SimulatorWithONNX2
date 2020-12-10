#include <torch/script.h> // One-stop header.

#include <iostream>
#include <memory>
#include <math.h>

int main(int argc, const char* argv[]) {
  if (argc != 2) {
    std::cerr << "usage: example-app <path-to-exported-script-module>\n";
    return -1;
  }


  torch::jit::script::Module module;
  try {
    // Deserialize the ScriptModule from a file using torch::jit::load().
    module = torch::jit::load(argv[1]);
  }
  catch (const c10::Error& e) {
    std::cerr << "error loading the model\n";
    return -1;
  }

  // Create a vector of inputs.
  std::vector<torch::jit::IValue> inputs;
  inputs.push_back(torch::rand({9, 8}));

  std::cout << inputs << '\n';

  // Execute the model and turn its output into a tensor.
  at::Tensor output = module.forward(inputs).toTensor();
  std::cout << round(output.slice(/*dim=*/1, /*start=*/0, /*end=*/5)) << '\n';


  //std::cout << inputs;
}