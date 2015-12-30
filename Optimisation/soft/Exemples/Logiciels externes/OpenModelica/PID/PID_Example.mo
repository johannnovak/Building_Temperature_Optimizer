model PID_Example
  extends Modelica.Icons.Example;
  // Adding parameters inputs from a simple text file
  // read from file
  parameter String file = Modelica.Utilities.Files.loadResource("E:/UTBM/ST40/WFA/WFA1/bin/Debug/PID/Parameters.txt");
  // parameters to set
  parameter Real x0 = Modelica.Utilities.Examples.readRealParameter(file, "x0") "1eme variable";
  parameter Real x1 = Modelica.Utilities.Examples.readRealParameter(file, "x1") "2eme variable";
  parameter Real x2 = Modelica.Utilities.Examples.readRealParameter(file, "x2") "3eme variable";
  parameter Modelica.SIunits.Angle driveAngle = 1.57 "Reference distance to move";
  Modelica.Blocks.Continuous.LimPID PI(k = x0, Ti = x1, yMax = 12, Ni = 0.1, initType = Modelica.Blocks.Types.InitPID.SteadyState, limitsAtInit = false, controllerType = Modelica.Blocks.Types.SimpleController.PID, Td = x2, wd = 1) annotation(Placement(transformation(extent = {{-56, -20}, {-36, 0}}, rotation = 0)));
  Modelica.Mechanics.Rotational.Components.Inertia inertia1(phi(fixed = true, start = 0), J = 1.5, a(fixed = true, start = 0)) annotation(Placement(transformation(extent = {{2, -20}, {22, 0}}, rotation = 0)));
  Modelica.Mechanics.Rotational.Sources.Torque torque annotation(Placement(transformation(extent = {{-25, -20}, {-5, 0}}, rotation = 0)));
  Modelica.Mechanics.Rotational.Components.SpringDamper spring(c = 5000, d = 10, stateSelect = StateSelect.prefer, w_rel(fixed = true)) annotation(Placement(transformation(extent = {{32, -20}, {52, 0}}, rotation = 0)));
  Modelica.Mechanics.Rotational.Components.Inertia inertia2(J = 3) annotation(Placement(transformation(extent = {{60, -20}, {80, 0}}, rotation = 0)));
  Modelica.Blocks.Sources.KinematicPTP kinematicPTP(startTime = 0.25, deltaq = {driveAngle}, qd_max = {1}, qdd_max = {2}) annotation(Placement(transformation(extent = {{-92, 20}, {-72, 40}}, rotation = 0)));
  Modelica.Blocks.Continuous.Integrator integrator(initType = Modelica.Blocks.Types.Init.InitialState) annotation(Placement(transformation(extent = {{-63, 20}, {-43, 40}}, rotation = 0)));
  Modelica.Mechanics.Rotational.Sensors.SpeedSensor speedSensor annotation(Placement(transformation(extent = {{22, -50}, {2, -30}}, rotation = 0)));
  Modelica.Mechanics.Rotational.Sources.ConstantTorque loadTorque(tau_constant = 3, useSupport = false) annotation(Placement(transformation(extent = {{98, -15}, {88, -5}}, rotation = 0)));
  Modelica.Blocks.Math.Add add1(k2 = -1) annotation(Placement(visible = true, transformation(origin = {-60, -80}, extent = {{-10, -10}, {10, 10}}, rotation = 0)));
  Modelica.Blocks.Math.Abs abs1 annotation(Placement(visible = true, transformation(origin = {-20, -80}, extent = {{-10, -10}, {10, 10}}, rotation = 0)));
  Modelica.Blocks.Math.Product product1 annotation(Placement(visible = true, transformation(origin = {20, -80}, extent = {{-10, -10}, {10, 10}}, rotation = 0)));
  Modelica.Blocks.Continuous.Integrator integrator1 annotation(Placement(visible = true, transformation(origin = {60, -80}, extent = {{-10, -10}, {10, 10}}, rotation = 0)));
  Modelica.Blocks.Interfaces.RealOutput fobj annotation(Placement(visible = true, transformation(origin = {100, -80}, extent = {{-10, -10}, {10, 10}}, rotation = 0), iconTransformation(origin = {100, -80}, extent = {{-10, -10}, {10, 10}}, rotation = 0)));
initial equation
  der(spring.w_rel) = 0;
equation
  connect(integrator1.y, fobj) annotation(Line(points = {{71, -80}, {92.4119, -80}, {92.4119, -79.9458}, {92.4119, -79.9458}}, color = {0, 0, 127}));
  connect(product1.y, integrator1.u) annotation(Line(points = {{31, -80}, {47.0588, -80}, {47.0588, -79.9189}, {47.0588, -79.9189}}, color = {0, 0, 127}));
  connect(abs1.y, product1.u2) annotation(Line(points = {{-9, -80}, {-2.43408, -80}, {-2.43408, -86.0041}, {7.30223, -86.0041}, {7.30223, -86.0041}}, color = {0, 0, 127}));
  connect(abs1.y, product1.u1) annotation(Line(points = {{-9, -80}, {-2.43408, -80}, {-2.43408, -74.2394}, {8.51927, -74.2394}, {8.51927, -74.2394}}, color = {0, 0, 127}));
  connect(add1.y, abs1.u) annotation(Line(points = {{-49, -80}, {-31.643, -80}, {-31.643, -80.3245}, {-31.643, -80.3245}}, color = {0, 0, 127}));
  connect(add1.u2, PI.u_m) annotation(Line(points = {{-72, -86}, {-92.4949, -86}, {-92.4949, -50.7099}, {-45.8418, -50.7099}, {-45.8418, -23.1237}, {-45.8418, -23.1237}}, color = {0, 0, 127}));
  connect(add1.u1, PI.u_s) annotation(Line(points = {{-72, -74}, {-84.787, -74}, {-84.787, -10.142}, {-58.4178, -10.142}, {-58.4178, -10.142}}, color = {0, 0, 127}));
  connect(spring.flange_b, inertia2.flange_a) annotation(Line(points = {{52, -10}, {60, -10}}, color = {0, 0, 0}));
  connect(inertia1.flange_b, spring.flange_a) annotation(Line(points = {{22, -10}, {32, -10}}, color = {0, 0, 0}));
  connect(torque.flange, inertia1.flange_a) annotation(Line(points = {{-5, -10}, {2, -10}}, color = {0, 0, 0}));
  connect(kinematicPTP.y[1], integrator.u) annotation(Line(points = {{-71, 30}, {-65, 30}}, color = {0, 0, 127}));
  connect(speedSensor.flange, inertia1.flange_b) annotation(Line(points = {{22, -40}, {22, -10}}, color = {0, 0, 0}));
  connect(loadTorque.flange, inertia2.flange_b) annotation(Line(points = {{88, -10}, {80, -10}}, color = {0, 0, 0}));
  connect(PI.y, torque.tau) annotation(Line(points = {{-35, -10}, {-27, -10}}, color = {0, 0, 127}));
  connect(speedSensor.w, PI.u_m) annotation(Line(points = {{1, -40}, {-46, -40}, {-46, -22}}, color = {0, 0, 127}));
  connect(integrator.y, PI.u_s) annotation(Line(points = {{-42, 30}, {-37, 30}, {-37, 11}, {-67, 11}, {-67, -10}, {-58, -10}}, color = {0, 0, 127}));
  annotation(Icon(coordinateSystem(extent = {{-100, -100}, {100, 100}}, preserveAspectRatio = true, initialScale = 0.1, grid = {2, 2})), experiment(StopTime = 4), Documentation(info = "<html>

                                   <p>
                                   This is a simple drive train controlled by a PID controller:
                                   </p>

                                   <ul>
                                   <li> The two blocks \"kinematic_PTP\" and \"integrator\" are used to generate
                                        the reference speed (= constant acceleration phase, constant speed phase,
                                        constant deceleration phase until inertia is at rest). To check
                                        whether the system starts in steady state, the reference speed is
                                        zero until time = 0.5 s and then follows the sketched trajectory.</li>

                                   <li> The block \"PI\" is an instance of \"Blocks.Continuous.LimPID\" which is
                                        a PID controller where several practical important aspects, such as
                                        anti-windup-compensation has been added. In this case, the control block
                                        is used as PI controller.</li>

                                   <li> The output of the controller is a torque that drives a motor inertia
                                        \"inertia1\". Via a compliant spring/damper component, the load
                                        inertia \"inertia2\" is attached. A constant external torque of 10 Nm
                                        is acting on the load inertia.</li>
                                   </ul>

                                   <p>
                                   The PI controller settings included \"limitAtInit=false\", in order that
                                   the controller output limits of 12 Nm are removed from the initialization
                                   problem.
                                   </p>

                                   <p>
                                   The PI controller is initialized in steady state (initType=SteadyState)
                                   and the drive shall also be initialized in steady state.
                                   However, it is not possible to initialize \"inertia1\" in SteadyState, because
                                   \"der(inertia1.phi)=inertia1.w=0\" is an input to the PI controller that
                                   defines that the derivative of the integrator state is zero (= the same
                                   condition that was already defined by option SteadyState of the PI controller).
                                   Furthermore, one initial condition is missing, because the absolute position
                                   of inertia1 or inertia2 is not defined. The solution shown in this examples is
                                   to initialize the angle and the angular acceleration of \"inertia1\".
                                   </p>

                                   <p>
                                   In the following figure, results of a typical simulation are shown:
                                   </p>

                                   <img src=\"modelica://Modelica/Resources/Images/Blocks/PID_controller.png\"
                                        alt=\"PID_controller.png\"><br>

                                   <img src=\"modelica://Modelica/Resources/Images/Blocks/PID_controller2.png\"
                                        alt=\"PID_controller2.png\">

                                   <p>
                                   In the upper figure the reference speed (= integrator.y) and
                                   the actual speed (= inertia1.w) are shown. As can be seen,
                                   the system initializes in steady state, since no transients
                                   are present. The inertia follows the reference speed quite good
                                   until the end of the constant speed phase. Then there is a deviation.
                                   In the lower figure the reason can be seen: The output of the
                                   controller (PI.y) is in its limits. The anti-windup compensation
                                   works reasonably, since the input to the limiter (PI.limiter.u)
                                   is forced back to its limit after a transient phase.
                                   </p>

                                   </html>"), Diagram(coordinateSystem(extent = {{-100, -100}, {100, 100}}, preserveAspectRatio = true, initialScale = 0.1, grid = {2, 2}), graphics = {Rectangle(lineColor = {255, 0, 0}, extent = {{-99, 48}, {-32, 8}}), Text(lineColor = {255, 0, 0}, extent = {{-98, 59}, {-31, 51}}, textString = "reference speed generation"), Text(origin = {0, 2.43902}, lineColor = {255, 0, 0}, extent = {{-98, -46}, {-60, -52}}, textString = "PI controller"), Line(points = {{-76, -44}, {-57, -23}}, color = {255, 0, 0}, arrow = {Arrow.None, Arrow.Filled}), Rectangle(lineColor = {255, 0, 0}, extent = {{-25, 6}, {99, -50}}), Text(lineColor = {255, 0, 0}, extent = {{4, 14}, {71, 7}}, textString = "plant (simple drive train)"), Rectangle(origin = {-62.8283, -48.3502}, lineColor = {255, 0, 0}, extent = {{-23.9899, -15.2121}, {169.37, -50.3367}}), Text(origin = {-8.96, -69.56}, lineColor = {255, 0, 0}, extent = {{4, 14}, {71, 7}}, textString = "Objective Function (f=x^2)")}));
end PID_Example;