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

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(COL153,'P') or contains(COL153,'C')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before($Symbol,1)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-after(normalize-space(COL155),$UnderlyingSymbol),5,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-after(normalize-space(COL155),$UnderlyingSymbol),3,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(normalize-space(COL155),$UnderlyingSymbol),1,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-after(normalize-space(COL155),$UnderlyingSymbol),7,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring(substring-after(normalize-space(COL155),$UnderlyingSymbol),8),'#.00')"/>
			</xsl:variable>


			<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="number($ExpiryMonth)"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>



			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>

		</xsl:if>
	</xsl:template>
	<xsl:template name="ConvertBBCodetoTicker">
		<xsl:param name="varBBCode"/>

		<xsl:variable name="varRoot">
			<xsl:choose>
				<xsl:when test="substring($varBBCode,2,1)='1' or substring($varBBCode,2,1)='2'">
					<xsl:value-of select="substring($varBBCode,2,1)"/>
				</xsl:when>
				<xsl:when test="substring($varBBCode,3,1)='1' or substring($varBBCode,3,1)='2'">
					<xsl:value-of select="substring($varBBCode,3,1)"/>
				</xsl:when>
				<xsl:when test="substring($varBBCode,4,1)='1' or substring($varBBCode,4,1)='2'">
					<xsl:value-of select="substring($varBBCode,4,1)"/>
				</xsl:when>
				<xsl:when test="substring($varBBCode,5,1)='1' or substring($varBBCode,5,1)='2'">
					<xsl:value-of select="substring($varBBCode,5,1)"/>
				</xsl:when>
				<xsl:when test="substring($varBBCode,6,1)='1' or substring($varBBCode,6,1)='2'">
					<xsl:value-of select="substring($varBBCode,6,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
			<!-- <xsl:value-of select="substring-before($varBBCode,'1')"/> -->
		</xsl:variable>
		<xsl:variable name="varUnderlying">
			<xsl:value-of select="substring-before($varBBCode,$varRoot)"/>
		</xsl:variable>

		<xsl:variable name="varExYear">
			<xsl:value-of select="substring(substring-after($varBBCode,$varUnderlying),1,2)"/>
		</xsl:variable>

		<xsl:variable name="varStrike">
			<xsl:value-of select="format-number(substring(substring-after($varBBCode,$varUnderlying),8), '#.00')"/>
		</xsl:variable>

		<xsl:variable name="varExDay">
			<xsl:value-of select="substring(substring-after($varBBCode,$varUnderlying),5,2)"/>
		</xsl:variable>

		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(substring-after($varBBCode,$varUnderlying),7,1)"/>
		</xsl:variable>
		<xsl:variable name="varExpiryDay">
			<xsl:choose>
				<xsl:when test="substring($varExDay,1,1)= '0'">
					<xsl:value-of select="substring($varExDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varExDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varMonthCode">
			<xsl:value-of select="substring(substring-after($varBBCode,$varUnderlying),3,2)"/>
		</xsl:variable>

		<xsl:variable name="varMonth">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="Month" select="$varMonthCode"/>
				<xsl:with-param name="PutOrCall" select="$PutORCall"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:value-of select="normalize-space(concat('O:', $varUnderlying, ' ', $varExYear,$varMonth,$varStrike,'D',$varExpiryDay))"/>
	</xsl:template>


	<xsl:template match="/">
		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL58"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity)">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Fidelity'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL75"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>




						<xsl:variable name="Symbol" select="normalize-space(COL155)"/>

						<xsl:variable name="UnderlyingSymbol">
							<xsl:value-of select="substring-before($Symbol,1)"/>
						</xsl:variable>

						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="COL153='C' or COL153='P'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Asset='EquityOption'">
									<xsl:call-template name="ConvertBBCodetoTicker">
										<xsl:with-param name="varBBCode" select="COL155"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>
								
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>




						<xsl:variable name="PB_FUND_NAME" select="COL12"/>
						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="COL101"/>
						</xsl:variable>
						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
						</xsl:variable>

						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_BROKER_ID)">
									<xsl:value-of select="$PRANA_BROKER_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
							<!--<xsl:value-of select="1"/>-->
						</CounterPartyID>



						<xsl:variable name="Quantitye">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL58 div 100000"/>
							</xsl:call-template>
						</xsl:variable>


						<Quantity>
							<xsl:choose>
								<xsl:when test="$Quantitye &gt; 0">
									<xsl:value-of select="$Quantitye"/>
								</xsl:when>

								<xsl:when test="$Quantitye &lt; 0">
									<xsl:value-of select="$Quantitye * 1"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL59 div 1000000000"/>
							</xsl:call-template>
						</xsl:variable>
						<AvgPX>
							<xsl:choose>
								<xsl:when test="$AvgPrice &gt; 0">
									<xsl:value-of select="$AvgPrice"/>

								</xsl:when>
								<xsl:when test="$AvgPrice &lt; 0">
									<xsl:value-of select="$AvgPrice * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</AvgPX>



						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL66 div 100"/>
							</xsl:call-template>
						</xsl:variable>
						<Commission>
							<xsl:choose>
								<xsl:when test="$Commission &gt; 0">
									<xsl:value-of select="$Commission"/>
								</xsl:when>
								<xsl:when test="$Commission &lt; 0">
									<xsl:value-of select="$Commission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="OrfFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL68 div 100"/>
							</xsl:call-template>
						</xsl:variable>
						<SecFee>
							<xsl:choose>
								<xsl:when test="$OrfFee &gt; 0">
									<xsl:value-of select="$OrfFee"/>
								</xsl:when>
								<xsl:when test="$OrfFee &lt; 0">
									<xsl:value-of select="$OrfFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</SecFee>

						<xsl:variable name="ClearingFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL69 div 100"/>
							</xsl:call-template>
						</xsl:variable>
						<OrfFee>
							<xsl:choose>
								<xsl:when test="$ClearingFee &gt; 0">
									<xsl:value-of select="$ClearingFee"/>
								</xsl:when>
								<xsl:when test="$ClearingFee &lt; 0">
									<xsl:value-of select="$ClearingFee * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrfFee>

						



						<xsl:variable name="AdditionalFee1">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL67 div 100"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="AdditionalFee2">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL70 div 100"/>
							</xsl:call-template>
						</xsl:variable>





						<xsl:variable name="OtherBrokerFees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$AdditionalFee1 + $AdditionalFee2"/>
							</xsl:call-template>
						</xsl:variable>
						<OtherBrokerFees>
							<xsl:choose>
								<xsl:when test="$OtherBrokerFees &gt; 0">
									<xsl:value-of select="$OtherBrokerFees"/>
								</xsl:when>
								<xsl:when test="$OtherBrokerFees &lt; 0">
									<xsl:value-of select="$OtherBrokerFees * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</OtherBrokerFees>
						
						
							<xsl:variable name="Side" select="normalize-space(COL3)"/>
					
						<Side>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:choose>
										<xsl:when test="contains(COL79,'CLOSING')">
											<xsl:choose>
												<xsl:when test="$Side='B'">
													<xsl:value-of select="'Buy to Close'"/>
												</xsl:when>
												<xsl:when test="$Side='S'">
													<xsl:value-of select="'Sell to Close'"/>
												</xsl:when>
											</xsl:choose>
										</xsl:when>
										<xsl:when test="contains(COL79,'OPENING')">
											<xsl:choose>
												<xsl:when test="$Side='B'">
													<xsl:value-of select="'Buy to Open'"/>
												</xsl:when>
												<xsl:when test="$Side='S'">
													<xsl:value-of select="'Sell to Open'"/>
												</xsl:when>
											</xsl:choose>
										</xsl:when>
									</xsl:choose>
								</xsl:when>												
								
															
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$Side='B' and (COL13='2' or COL13='1')">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>

										<xsl:when test="$Side='S'and (COL13='2' or COL13='1')">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>

										<xsl:when test="$Side='B'and COL13='3'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>

										<xsl:when test="$Side='S'and COL13='3'">
											<xsl:value-of select="'Sell Short'"/>
										</xsl:when>


										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>

						</Side>

						<xsl:variable name="NetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL71 div 100"/>
							</xsl:call-template>
						</xsl:variable>
						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="$NetNotionalValue &gt; 0">
									<xsl:value-of select="$NetNotionalValue"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValue &lt; 0">
									<xsl:value-of select="$NetNotionalValue * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>




						<xsl:variable name="Year">
							<xsl:value-of select="substring(COL4,1,4)"/>
						</xsl:variable>
						<xsl:variable name="Month">
							<xsl:value-of select="substring(COL4,5,2)"/>
						</xsl:variable>
						<xsl:variable name="Day">
							<xsl:value-of select="substring(COL4,7,2)"/>
						</xsl:variable>
						<Date>
							<xsl:value-of select ="concat($Month,'/',$Day,'/',$Year)"/>
						</Date>

						<xsl:variable name="Year1">
							<xsl:value-of select="substring(COL5,1,4)"/>
						</xsl:variable>
						<xsl:variable name="Month1">
							<xsl:value-of select="substring(COL5,5,2)"/>
						</xsl:variable>
						<xsl:variable name="Day1">
							<xsl:value-of select="substring(COL5,7,2)"/>
						</xsl:variable>
						<SettlementDate>
							<xsl:value-of select ="concat($Month1,'/',$Day1,'/',$Year1)"/>
						</SettlementDate>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


