module D1
import Base.*
import Base.+

  type Point
    x
    y
  end

  function *(point::Point, number::Int64)
    return Point(point.x * number, point.y * number)
  end

  function +(point::Point, point2::Point)
    return Point(point.x + point2.x, point.y + point2.y)
  end

  function rotate(vector,direction)
    if direction == 'R'
      return circshift(vector,-1)
    end

    if direction == 'L'
      return circshift(vector,1)
    end
  end

 function get_move(move)
   move_direction = move[1]
   move_size = parse(Int,move[2:end])
   return move_direction, move_size
 end

  function walk_path(path)
    start_coordinate = Point(0,0)
    step_vector = [Point(0,1),Point(1,0),Point(0,-1),Point(-1,0)]

    coordinate = Point(0,0)
    for move in split(path, ',')
      move_direction, move_size = get_move(move)
      step_vector = rotate(step_vector,move_direction)
      coordinate += step_vector[1] * move_size
    end

    manhattan_distance = abs(coordinate.x - start_coordinate.x) + abs(coordinate.y - start_coordinate.y)
    println("Manhatton distance from start to position is: ", manhattan_distance)
    return manhattan_distance
  end

  function walk(path)
    path = split(path, ",")
    pos = [0; 0]
    d = [0; 1]
    R(d) = [d[2]; -d[1]] # the rotate function
    L(d) = -R(d)
    for instruction in path
      println("d: ", d)
      d = instruction[1] == 'L' ? L(d) : R(d)
      mov = parse(Int, instruction[2:end])
      pos += d * mov
      println("pos: ", pos)
    end
    return sum(abs.(pos)) # norm(v, 1) returns float, and it bothers me
  end

  function test()
    @assert walk_path("R2,L3") == 5
    @assert walk_path("R2,R2,R2") == 2
    @assert walk_path("R5,L5,R5,R3") == 12
    @assert walk_path("R8,R4,R4,R8") == 8
  end

  function test2()
    #@assert walk("R2,L3") == 5
    @assert walk("R2,R2,R2,R2") == 0
    #@assert walk("R5,L5,R5,R3") == 12
    #@assert walk("R8,R4,R4,R8") == 8
  end

test()

end
