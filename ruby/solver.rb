class Solver
    def self.solve(quiz)
        sudoku = Sudoku.new
        for i in 0...quiz.length do
            if quiz[i] != 0 then
                sudoku.update(i, quiz[i])
            end
        end
        solveInternal(sudoku)
    end

    private

    def self.solveInternal(sudoku)
        updated = true
        solved = false

        while updated do
            updated = false
            solved = true

            for i in 0...(9*9) do
                if sudoku.answer[i] != 0 then
                    next
                end
                solved = false

                candidate = sudoku.getCandidateBit(i)
                if candidate == 0 then
                    return nil
                end

                lowest = lowestOneBit(candidate)
                if candidate == lowest then
                    sudoku.update(i, toNum(lowest))
                    updated = true
                end
            end
        end

        if solved then
            return sudoku.answer
        end

        count = 9999
        index = 0
        for i in 0...sudoku.answer.length do
            if sudoku.answer[i] != 0 then
                next
            end

            c = bitCount(sudoku.getCandidateBit(i))
            if c < count then
                count = candidate
                index = i
            end
        end

        candidate = sudoku.getCandidateBit(index)
        for i in 0...9 do
            bit = candidate & 1 << i
            if bit == 0 then
                next
            end

            newSudoku = Sudoku.new(sudoku)
            newSudoku.update(index, i + 1)

            answer = solveInternal(newSudoku)
            if answer != nil then
                return answer
            end
        end

        return nil
    end

    def self.bitCount(value)
        sum = 0
        for i in 0...9 do
            if (value & 1 << i) != 0 then
                sum += 1
            end
        end

        return sum
    end

    def self.lowestOneBit(i)
        i & -i
    end

    def self.toNum(bit)
		case (bit) 
		when 1
			return 1
		when 2
			return 2
		when 4
			return 3
		when 8
			return 4
		when 16
			return 5
		when 32
			return 6
		when 64
			return 7
		when 128
			return 8
		when 256
			return 9
        else
			raise "Invalid bit. [#{bit}]"
		end
    end

    class Sudoku
        attr_reader :answer, :rows, :columns, :regions

        def initialize(other = nil)
            if other.nil? then
                @answer = Array.new(9*9, 0)
                @rows = Array.new(9, 0)
                @columns = Array.new(9, 0)
                @regions = Array.new(9, 0)
            else
                @answer = other.answer.dup
                @rows = other.rows.dup
                @columns = other.columns.dup
                @regions = other.regions.dup
            end
        end

        def update(index, number)
            @answer[index] = number;

            r = index/9
            c = index%9
            value = 1 << (number - 1)
            @rows[r] |= value
            @columns[c] |= value
            @regions[(r/3)*3 + (c/3)] |= value
        end

        def getCandidateBit(index)
            r = index/9
            c = index%9

            bit = 0
            bit |= @rows[r]
            bit |= @columns[c]
            bit |= @regions[(r/3)*3 + (c/3)]

            return ~bit & 0x1FF
        end
    end
end