<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
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

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<xsl:param name="varOptionPutCall"/>
		<xsl:choose>
			<xsl:when  test ="$varMonth=1 and $varOptionPutCall='P'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=2 and $varOptionPutCall='P'">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=3 and $varOptionPutCall='P'">
				<xsl:value-of select ="'O'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=4 and $varOptionPutCall='P'">
				<xsl:value-of select ="'P'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=5 and $varOptionPutCall='P'">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=6 and $varOptionPutCall='P'">
				<xsl:value-of select ="'R'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=7 and $varOptionPutCall='P'">
				<xsl:value-of select ="'S'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=8 and $varOptionPutCall='P'">
				<xsl:value-of select ="'T'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=9 and $varOptionPutCall='P'">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=10 and $varOptionPutCall='P'">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=11 and $varOptionPutCall='P'">
				<xsl:value-of select ="'W'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=12 and $varOptionPutCall='P'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=1 and $varOptionPutCall='C'">
				<xsl:value-of select ="'A'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=2 and $varOptionPutCall='C'">
				<xsl:value-of select ="'B'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=3 and $varOptionPutCall='C'">
				<xsl:value-of select ="'C'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=4 and $varOptionPutCall='C'">
				<xsl:value-of select ="'D'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=5 and $varOptionPutCall='C'">
				<xsl:value-of select ="'E'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=6 and $varOptionPutCall='C'">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=7 and $varOptionPutCall='C'">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=8 and $varOptionPutCall='C'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=9 and $varOptionPutCall='C'">
				<xsl:value-of select ="'I'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=10 and $varOptionPutCall='C'">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=11 and $varOptionPutCall='C'">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when  test ="$varMonth=12 and $varOptionPutCall='C'">
				<xsl:value-of select ="'L'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>

			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varCostBasis">
					<xsl:value-of select="COL5"/>
				</xsl:variable>

				<xsl:if test ="number(COL3)">

					<PositionMaster>
						<!--   Fund -->
						<!--fundname section-->
						<!--<xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL2"/>
            </xsl:variable>
            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='GS']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>-->

						<xsl:variable name="PB_Symbol">
							<xsl:value-of select = "COL4"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="varNetPosition">
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<xsl:variable name="PB_CountnerParty" select="COL6"/>
						<xsl:variable name="PRANA_CounterPartyID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= 'GS']/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
						</xsl:variable>

						<xsl:variable name="varCommission">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<xsl:variable name="varFees">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<xsl:variable name="varStampDuty">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<!--<xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <AccountName>
                  <xsl:value-of select='$PB_FUND_NAME'/>
                </AccountName>
              </xsl:when>
              <xsl:otherwise>
                <AccountName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </AccountName>
              </xsl:otherwise>
            </xsl:choose>-->

						<AccountName>
							<xsl:value-of select='""'/>
						</AccountName>

						<PositionStartDate>
							<xsl:value-of select="''"/>
						</PositionStartDate>


						<xsl:choose>
							<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
								<Symbol>
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</Symbol>
							</xsl:when>
							<xsl:when test="string-length(COL4) &gt; 10">
								<xsl:variable name = "PB_COMPANY" >
									<xsl:value-of select="normalize-space(COL4)"/>
								</xsl:variable>

								<xsl:variable name="PRANA_SYMBOL">
									<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='GS']/SymbolData[@PBCompanyName=$PB_COMPANY]/@PranaSymbol"/>
								</xsl:variable>


								<xsl:variable name ="varUnderlyingSymbol">
									<xsl:value-of select ="substring-before(normalize-space(COL4),' ')"/>
								</xsl:variable>

								<xsl:variable name="varEXDate">
									<xsl:value-of select="COL9"/>
								</xsl:variable>

								<xsl:variable name="varYear">
									<xsl:value-of select="substring(substring-after(substring-after($varEXDate,'/'),'/'),3,2)"/>
								</xsl:variable>

								<xsl:variable name="varMonth">
									<xsl:value-of select="substring-before($varEXDate,'/')"/>
								</xsl:variable>


								<xsl:variable name="varExDay">
									<xsl:value-of select="substring-before(substring-after($varEXDate,'/'),'/')"/>
								</xsl:variable>


								<xsl:variable name="varOptionPutCall">
									<xsl:value-of select ="substring(substring-after(substring-after(substring-after(COL4,' '),' '),' '),1,1)"/>
								</xsl:variable>


								<xsl:variable name ="varStrikePrice">
									<xsl:value-of select ="format-number(substring-before(substring-after(substring-after(COL4,' '),' '),' '),'#.00')"/>
								</xsl:variable>

								<xsl:variable name="varMonthCode">
									<xsl:call-template name="MonthCode">
										<xsl:with-param name="varMonth" select="number($varMonth)"/>
										<xsl:with-param name="varOptionPutCall" select="$varOptionPutCall"/>
									</xsl:call-template>
								</xsl:variable>


								<!--<PBSymbol>
							<xsl:value-of select="concat($varYear,' ',$varMonth,' ',$varExDay,' ',$varOptionPutCall,' ',$varStrikePrice)"/>
						</PBSymbol>-->



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
										<xsl:when test ="number($varMonth) and number($varYear)">
											<xsl:value-of select='my:Now(number(concat(20,$varYear)),number($varMonth))'/>
										</xsl:when>
									</xsl:choose>

								</xsl:variable>

								<xsl:variable name="varOptionSymbol">

									<xsl:choose>
										<xsl:when test="substring(substring-after($varThirdFriday,'/'),1,2) = ($varExDay - 1)">
											<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number(($varStrikePrice),'#.00'))"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat('O:',$varUnderlyingSymbol,' ',$varYear,$varMonthCode,format-number($varStrikePrice,'#.00'),'D',$varDays)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<Symbol>
									<xsl:value-of select="$varOptionSymbol"/>
								</Symbol>
							</xsl:when>
							<xsl:otherwise>
								<Symbol>
									<xsl:value-of select ="COL4"/>
								</Symbol>
							</xsl:otherwise>
						</xsl:choose>


						<PBSymbol>
							<xsl:value-of select="COL4"/>
						</PBSymbol>


						<!--QUANTITY-->

						<xsl:choose>
							<xsl:when test="$varNetPosition &lt; 0">
								<NetPosition>
									<xsl:value-of select="$varNetPosition * (-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test="$varNetPosition &gt; 0">
								<NetPosition>
									<xsl:value-of select="$varNetPosition"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>

						<!--Side-->

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="COL2 = 'Buy'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL2 = 'Sell'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="COL2 = 'Cover'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="COL2 = 'Short'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<CostBasis>
							<xsl:choose>
								<xsl:when test ="number($varCostBasis) &gt; 0">
									<xsl:value-of select="$varCostBasis"/>
								</xsl:when>
								<xsl:when test ="number($varCostBasis) &lt; 0">
									<xsl:value-of select="$varCostBasis*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_CounterPartyID)">
									<xsl:value-of select="$PRANA_CounterPartyID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>

						<!--<Commission>
              <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>

            <Fees>
              <xsl:choose>
                <xsl:when test="$varFees &gt; 0">
                  <xsl:value-of select="$varFees"/>
                </xsl:when>
                <xsl:when test="$varFees &lt; 0">
                  <xsl:value-of select="$varFees*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>-->


					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
