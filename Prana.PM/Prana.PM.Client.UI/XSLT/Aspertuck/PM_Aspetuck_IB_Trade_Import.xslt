<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here"
	xmlns:my1="put-your-namespace-uri-here"
	xmlns:my2="put-your-namespace-uri-here"
	xmlns:my5="put-your-namespace-uri-here"
	xmlns:my3="put-your-namespace-uri-here">

	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>

	<msxsl:script language="C#" implements-prefix="my5">
		<msxsl:assembly name="System.Data"/>
		<msxsl:using namespace="System.Data"/>

		public String DateCheck(String trade, String settlement)
		{
		DateTime tradeDate = Convert.ToDateTime(trade);
		DateTime settlementDate = Convert.ToDateTime(settlement);
		DataSet ds = new DataSet();
		ds.ReadXml(@"C:/Nirvana/CHMWProd_Phase2/Client Release/MappingFiles/ReconMappingXml/HolidayMapping.xml");
		int count = 0;

		for (DateTime dt = tradeDate.AddDays(1); DateTime.Compare(dt, settlementDate) &lt;= 0; dt = dt.AddDays(1))
		{
		if (!(dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday))
		{
		bool flag = false;
		for (int i = 0; i &lt; ds.Tables[1].Rows.Count; i++)
		{
		if(DateTime.Compare(dt,Convert.ToDateTime(ds.Tables[1].Rows[i][1].ToString())) == 0)
		{
		flag = true;
		}
		}
		if (!flag)
		{
		count++;
		}
		}
		}
		if (count &lt; 3)
		{
		return "Spot";
		}
		else
		{
		return "Forward";
		}
		}
	</msxsl:script>
	
	<msxsl:script language="C#" implements-prefix="my3">
	public static string SpotOrForward(String tradeDate, String settlementDate)
	{
	DateTime trade = Convert.ToDateTime(tradeDate);
	DateTime settlement = Convert.ToDateTime(settlementDate);
	int count = 0;
	for (DateTime dt = trade.AddDays(1); (dt.Date.CompareTo(settlement.Date)) &lt;= 0; dt = dt.AddDays(1))
	{
	if (!(dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday))
	{
	count++;
	}
	}
	if (count &lt; 3)
		{
		return "1";
		}
		if (count > 2)
		{
		return "3";
		}
		return null;
		}
	</msxsl:script>
	
	<msxsl:script language="C#" implements-prefix="my1">
		public double DateDiff(DateTime StartDate, DateTime EndDate)
		{

		return (EndDate - StartDate).TotalDays;
		}
	</msxsl:script>

	<msxsl:script language="C#" implements-prefix="my2">

		public string Now(int year, int month, int date)
		{
		DateTime weekEnd= new DateTime(year, month, date);
		while (weekEnd.DayOfWeek == DayOfWeek.Saturday || weekEnd.DayOfWeek == DayOfWeek.Sunday)
		{
		weekEnd = weekEnd.AddDays(1);
		}
		return weekEnd.ToString();
		}

	</msxsl:script>

	<xsl:template name ="MonthCode">
		<xsl:param name ="varMonth"/>
		<xsl:param name ="varPutCall"/>
		<xsl:choose>
			<xsl:when  test ="$varMonth=1 and $varPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=2 and $varPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=3 and $varPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=4 and $varPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=5 and $varPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=6 and $varPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=7 and $varPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=8 and $varPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=9 and $varPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when  test =" $varMonth=10 and $varPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=11 and $varPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=12  and $varPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=1 and $varPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=2 and $varPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=3 and $varPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=4 and $varPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=5 and $varPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=6 and $varPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=7 and $varPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=8 and $varPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=9 and $varPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=10 and $varPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=11 and $varPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=12 and $varPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="varUnderLying"/>
		<xsl:param name="varEX"/>
		<xsl:param name="varputCall"/>
		<xsl:param name="varStrike"/>
		<xsl:param name="Asset"/>


		<xsl:variable name ="varUnderlyingSymbol">
			<xsl:choose>
				<xsl:when test="$Asset='OPT'">
					<xsl:value-of select ="$varUnderLying"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varYear">
			<xsl:choose>
				<xsl:when test="$Asset='OPT'">
					<xsl:value-of select ="substring(substring-after(substring-after($varEX,'/'),'/'),3,2)"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varMonth">
			<xsl:choose>
				<xsl:when test="$Asset='OPT'">
					<xsl:value-of select ="substring-before($varEX,'/')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varExDay">
			<xsl:choose>
				<xsl:when test="$Asset='OPT'">
					<xsl:value-of select ="substring-before(substring-after($varEX,'/'),'/')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varPutCall">
			<xsl:choose>
				<xsl:when test="$Asset='OPT'">
					<xsl:value-of select ="$varputCall"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>


		<xsl:variable name ="varStrikePrice">
			<xsl:choose>
				<xsl:when test="$Asset='OPT'">
					<xsl:value-of select ="format-number($varStrike,'#.00')"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="varMonthCode">
			<xsl:call-template name ="MonthCode">
				<xsl:with-param name ="varMonth" select ="number($varMonth)"/>
				<xsl:with-param name ="varPutCall" select="$varPutCall"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="varDays">
			<xsl:choose>
				<xsl:when test="substring($varExDay,1,1)='0'">
					<xsl:value-of select="substring($varExDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varExDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varThirdFriday">

			<xsl:choose>
				<xsl:when test="$Asset='OPT' and number($varYear) and number($varMonth)">
					<xsl:value-of select='my:Now(concat(20,$varYear),$varMonth)'/>
				</xsl:when>
			</xsl:choose>

		</xsl:variable>


		<xsl:choose>
			<xsl:when test="$Asset='OPT'">
				<xsl:choose>
					<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
						<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number(($varStrikePrice),'#.00'))"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number($varStrikePrice,'#.00'),'D',$varDays)"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
		</xsl:choose>
		<!--</xsl:param>-->
	</xsl:template>
	<xsl:template match="/">

		<DocumentElement>


			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name ="varQuantity">
					<xsl:choose>
						<xsl:when test="contains(COL26,'FX')">
							<xsl:choose>
								<xsl:when test="COL5=1">
									<xsl:value-of select="COL27"/>
								</xsl:when>
								<xsl:when test="COL5!=1">
									<xsl:value-of select="COL30"/>
								</xsl:when>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="number(COL27)"/>
						</xsl:otherwise>
					</xsl:choose>

				</xsl:variable>

				<xsl:if test ="number($varQuantity)">

					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'Aspetuck'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL7"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_SUFFIX_NAME" select="substring-after(COL16,'.')"/>

						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name ="Date" select="substring(COL18,7,2)"/>
						<xsl:variable name ="Month" select="substring(COL18,5,2)"/>
						<xsl:variable name ="Year" select="substring(COL18,1,4)"/>

						<xsl:variable name="varOption">
							<xsl:call-template name="Option">
								<xsl:with-param name="varUnderLying" select="COL14"/>
								<xsl:with-param name="varEX" select ="concat($Month,'/',$Date,'/',$Year)"/>
								<xsl:with-param name="varputCall" select="COL19"/>
								<xsl:with-param name="varStrike" select="COL17"/>
								<xsl:with-param name="Asset" select="COL4"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="Asset" select="COL4"/>

						<xsl:variable name="TradeDate">
							<xsl:value-of select ="concat(substring(COL22,5,2),'/',substring(COL22,7,2),'/',substring(COL22,1,4))"/>
						</xsl:variable>
						<xsl:variable name="TradeDate1">
							<xsl:value-of select ="concat(substring(COL22,5,2),'-',substring(COL22,7,2),'-',substring(COL22,1,4))"/>
						</xsl:variable>

						<xsl:variable name="SettleMentDate">
							<xsl:value-of select ="concat(substring(COL24,5,2),'/',substring(COL24,7,2),'/',substring(COL24,1,4))"/>
						</xsl:variable>
						
						<xsl:variable name="CheckDate">
							<xsl:value-of select="my2:Now(substring(COL22,1,4),substring(COL22,5,2),substring(COL22,7,2))"/>
						</xsl:variable>

						

						<xsl:variable name="Fridaycheck">
							<xsl:value-of select="my5:Now(substring(COL22,1,4),substring(COL22,5,2),(substring(COL22,7,2)+1))"/>
						</xsl:variable>

						<xsl:variable name="Final">
							<xsl:value-of select="my5:DateCheck($TradeDate,$SettleMentDate)"/>
						</xsl:variable>

						<xsl:variable name="SFinal">

							<xsl:value-of select="concat(substring-before(substring-after($Final,'-'),'-'),'/',substring-before(substring-after(substring-after($Final,'-'),'-'),'T'),'/',substring-before($Final,'-'))"/>
						</xsl:variable>


						<xsl:variable name="PositionSettlementDate">
							<xsl:choose>
								<xsl:when test="contains(COL26,'FX')">
									<xsl:value-of select="concat(substring(COL24,5,2),'/',substring(COL24,7,2),'/',substring(COL24,1,4))"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="PositionSettlementDate1">
							<xsl:value-of select="COL24"/>
						</xsl:variable>


						<!--<xsl:variable name ="varSymbolFX">
							<xsl:choose>
								<xsl:when test="contains(COL26,'FX')">
									<xsl:choose>

										<xsl:when test="COL5=1">
											<xsl:choose>
												<xsl:when test="$DIFFDate &gt; 2">
													<xsl:value-of select="concat(substring-before(COL6,'.'),'-','USD',' ',$PositionSettlementDate1)"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="concat(substring-before(COL6,'.'),'-','USD SPOT')"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:when test="COL5!=1">
											<xsl:choose>
												<xsl:when test="$DIFFDate &gt; 2">
													<xsl:value-of select="concat(substring-after(COL6,'.'),'-','USD',' ',$PositionSettlementDate1)"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="concat(substring-after(COL6,'.'),'-','USD SPOT')"/>
												</xsl:otherwise>
											</xsl:choose>

										</xsl:when>
									</xsl:choose>
								</xsl:when>
								
								
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$PRANA_SYMBOL_NAME!=''">
											<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
										</xsl:when>

										<xsl:when test="$Asset='OPT'">
											<xsl:value-of select="$varOption"/>
										</xsl:when>

										<xsl:when test="COL6!='*'">
											<xsl:value-of select="normalize-space(concat(COL6,$PRANA_SUFFIX_NAME))"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="$PB_SYMBOL_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>-->

						<xsl:variable name ="varSymbolFX">
							<xsl:choose>
								<xsl:when test="contains(COL26,'FX')">
									<xsl:choose>

										<xsl:when test="COL5=1">
											<xsl:choose>
												<xsl:when test="$Final='Forward'">
													<xsl:value-of select="concat(substring-before(COL6,'.'),'-','USD',' ',$PositionSettlementDate1)"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="concat(substring-before(COL6,'.'),'-','USD SPOT')"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:when test="COL5!=1">
											<xsl:choose>
												<xsl:when test="$Final='Forward'">
													<xsl:value-of select="concat(substring-after(COL6,'.'),'-','USD',' ',$PositionSettlementDate1)"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="concat(substring-after(COL6,'.'),'-','USD SPOT')"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$PRANA_SYMBOL_NAME!=''">
											<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
										</xsl:when>

										<xsl:when test="$Asset='OPT'">
											<xsl:value-of select="$varOption"/>
										</xsl:when>

										<xsl:when test="COL6!='*'">
											<xsl:value-of select="normalize-space(concat(COL6,$PRANA_SUFFIX_NAME))"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="$PB_SYMBOL_NAME"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>
						
						
						


						<xsl:variable name="Symbol">
							<xsl:value-of select="$varSymbolFX"/>
						</xsl:variable>

						<Symbol>
							<xsl:value-of select="$Symbol"/>
						</Symbol>

						

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL1"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<FundName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</FundName>

						<PositionStartDate>
							<xsl:value-of select="concat(substring(COL22,5,2),'/',substring(COL22,7,2),'/',substring(COL22,1,4))"/>
						</PositionStartDate>


						<PositionSettlementDate>
							<xsl:value-of select="$PositionSettlementDate"/>
						</PositionSettlementDate>


						<NetPosition>
							<xsl:choose>
								<xsl:when  test="number($varQuantity) &gt; 0">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:when test="number($varQuantity) &lt; 0">
									<xsl:value-of select="$varQuantity* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="varAvgPrice">
							<xsl:choose>
								<xsl:when test="contains(COL26,'FX')">
									<xsl:choose>
										<xsl:when test="COL5=1">
											<xsl:value-of select="translate(COL30,',','') div translate(COL27,',','')"/>
										</xsl:when>
										<xsl:when test="COL5!=1">
											<xsl:value-of select="translate(COL27,',','') div translate(COL30,',','')"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate(COL28,',','')"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ='number($varAvgPrice) &lt; 0'>
									<xsl:value-of select ="$varAvgPrice*-1"/>
								</xsl:when>
								<xsl:when test ='number($varAvgPrice) &gt; 0'>
									<xsl:value-of select ='$varAvgPrice'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<xsl:variable name="varSide" select="COL46"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="contains(COL26,'FX')">
									<xsl:choose>
										<xsl:when test="COL5=1">
											<xsl:choose>
												<xsl:when  test="$varSide='BUY' ">
													<xsl:value-of select="'1'"/>
												</xsl:when>
												<xsl:when  test="$varSide='SELL'">
													<xsl:value-of select="'2'"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="'0'"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:when test="COL5!=1">
											<xsl:choose>
												<xsl:when  test="$varSide='BUY' ">
													<xsl:value-of select="'2'"/>
												</xsl:when>
												<xsl:when  test="$varSide='SELL'">
													<xsl:value-of select="'1'"/>
												</xsl:when>

												<xsl:otherwise>
													<xsl:value-of select="'0'"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$Asset='OPT'">
											<xsl:choose>
												<xsl:when  test="$varSide='BUY' and COL35='O'">
													<xsl:value-of select="'A'"/>
												</xsl:when>
												<xsl:when  test="$varSide='SELL' and COL35='C'">
													<xsl:value-of select="'D'"/>
												</xsl:when>
												<xsl:when  test="$varSide='BUY' and COL35='C'">
													<xsl:value-of select="'B'"/>
												</xsl:when>
												<xsl:when  test="$varSide='SELL' and COL35='O'">
													<xsl:value-of select="'C'"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="'0'"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when  test="$varSide='BUY' and COL35='O'">
													<xsl:value-of select="'1'"/>
												</xsl:when>
												<xsl:when  test="$varSide='SELL' and COL35='O'">
													<xsl:value-of select="'5'"/>
												</xsl:when>
												<xsl:when  test="$varSide='BUY' and COL35='C'">
													<xsl:value-of select="'B'"/>
												</xsl:when>
												<xsl:when  test="$varSide='SELL' and COL35='C'">
													<xsl:value-of select="'2'"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="'0'"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>


						</SideTagValue>

						<xsl:variable name="varFX">
							<xsl:value-of select="number(COL5)"/>
						</xsl:variable>

						<FXRate>
							<xsl:choose>
								<xsl:when test ="number($varFX)">
									<xsl:value-of select="$varFX"/>
								</xsl:when >
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose >
						</FXRate>

						<xsl:variable name="Commission">
							<xsl:choose>
								<xsl:when test="contains(COL4,'STK')">
									<xsl:value-of select="translate(COL32,',','')"/>
								</xsl:when>
							</xsl:choose>

						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test ='number($Commission) &lt; 0'>
									<xsl:value-of select ="$Commission*-1"/>
								</xsl:when>
								<xsl:when test ='number($Commission) &gt; 0'>
									<xsl:value-of select ='$Commission'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="TransactionLevy">
							<xsl:value-of select="translate(COL31,',','')"/>
						</xsl:variable>

						<TransactionLevy>
							<xsl:choose>
								<xsl:when test ='number($TransactionLevy) &lt; 0'>
									<xsl:value-of select ="$TransactionLevy*-1"/>
								</xsl:when>
								<xsl:when test ='number($TransactionLevy) &gt; 0'>
									<xsl:value-of select ='$TransactionLevy'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TransactionLevy>

						<xsl:variable name="LongShort">

							<xsl:choose>
								<xsl:when test="$Asset='OPT'">
									<xsl:choose>
										<xsl:when  test="$varSide='BUY'">
											<xsl:value-of select="'Long'"/>
										</xsl:when>
										<xsl:when  test="$varSide='SELL'">
											<xsl:value-of select="'Short'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when  test="$varSide='BUY' and COL35='O'">
											<xsl:value-of select="'Long'"/>
										</xsl:when>
										<xsl:when  test="$varSide='SELL' and COL35='O'">
											<xsl:value-of select="'Short'"/>
										</xsl:when>
										<xsl:when  test="$varSide='BUY' and COL35='C'">
											<xsl:value-of select="'Short'"/>
										</xsl:when>
										<xsl:when  test="$varSide='SELL' and COL35='C'">
											<xsl:value-of select="'Long'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
							<!--<xsl:choose>
								<xsl:when test="$varSide='BUY' or $varSide='Buy to Open' or $varSide='SELL' or $varSide='Sell to Close'">
									<xsl:value-of select="'Long'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell short' or $varSide='Sell to Open' or $varSide='Buy to Close'">
									<xsl:value-of select="'Short'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>-->
						</xsl:variable>

						<xsl:variable name ="PRANA_STRATEGY_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/StrategyMapping.xml')/StrategyMapping/PB[@Name=$PB_NAME]/StrategyData[@Symbol=$Symbol and @LongShort = $LongShort]/@Strategy"/>
						</xsl:variable>

						<Strategy>
							<xsl:choose>
								<xsl:when test="$PRANA_STRATEGY_NAME!=''">
									<xsl:value-of select="$PRANA_STRATEGY_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Strategy>


						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

