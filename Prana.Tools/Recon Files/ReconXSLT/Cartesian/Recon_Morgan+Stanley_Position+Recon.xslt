<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    >
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber * (-1)"/>
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
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
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
		<xsl:variable name="UnderlyingSymbol">
			<xsl:value-of select="substring-before(normalize-space(COL84),' ')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL84),' '),' '),'/')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(normalize-space(COL84),' '),' '),'/'),'/')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL84),'/'),'/'),' ')"/>
		</xsl:variable>
		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL84),' '),' '),'/'),'/'),' '),1,1)"/>
		</xsl:variable>
		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(substring-before(substring(substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL84),' '),' '),'/'),'/'),' '),2),' '),'##.00')"/>
		</xsl:variable>
		<xsl:variable name="MonthCodeVar">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="Month" select="($ExpiryMonth)"/>
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
	</xsl:template>


	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">
				<xsl:variable name="PB_NAME">
					<xsl:value-of select="'MS'"/>
				</xsl:variable>

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL28"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name="PB_FUND_NAME" select="COL4"/>

				<xsl:variable name ="PRANA_FUND_NAME">
					<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
				</xsl:variable>

				<xsl:if test="number($Quantity) and (COL51!='CASH') and $PRANA_FUND_NAME!=''">

					<PositionMaster>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL6"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varAssetType">
							<xsl:choose>
								<xsl:when test="contains(COL51,'Put') or contains(COL51,'Call')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="varSedol">
							<xsl:value-of select="normalize-space(COL9)"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL8)"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varAssetType = 'EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="normalize-space(COL84)"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:when test="$varSedol!='' or $varSedol!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="$varSymbol!='' or $varSymbol!='*'">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:when test="COL8='EPIRA.ST'">
									<xsl:value-of select="'EPI_A-OMX'"/>
								</xsl:when>
								<xsl:when test="COL7='50058V107'">
									<xsl:value-of select="'KTNR'"/>
								</xsl:when>

								<xsl:when test="COL20='BMXLQJ4'">
									<xsl:value-of select="'HZM-LON'"/>
								</xsl:when>
								<xsl:when test="COL20='BP95GQ8'">
									<xsl:value-of select="'HZM-TC'"/>
								</xsl:when>

								<xsl:when test="COL7='8879919A7'">
									<xsl:value-of select="'TLRS'"/>
								</xsl:when>

								<xsl:when test="COL7='500583141'">
									<xsl:value-of select="'KTN/WTS 03/09/2025'"/>
								</xsl:when>
								<xsl:when test="COL8='149071082-149071082'">
									<xsl:value-of select="'BZO V25C95'"/>
								</xsl:when>

								

								

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						

						<SEDOL>
							<xsl:choose>
								<xsl:when test="$varSedol!='' or $varSedol!='*'">
									<xsl:value-of select="$varSedol"/>
								</xsl:when>
								<xsl:when test="$varSymbol!='' or $varSymbol!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>


						<xsl:variable name="PB_COUNTER_PARTY" select="COL60"/>

						<xsl:variable name="PRANA_COUNTER_PARTY">
							<xsl:value-of select ="document('../ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_COUNTER_PARTY]/@MLPBroker"/>
						</xsl:variable>

						<CounterParty>
							<xsl:choose>

								<xsl:when test ="$PRANA_COUNTER_PARTY!='' ">
									<xsl:value-of select ="$PRANA_COUNTER_PARTY"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_COUNTER_PARTY"/>
								</xsl:otherwise>

							</xsl:choose>
						</CounterParty>

						<!--<IDCOOptionSymbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>

                <xsl:when test="$AssetType='Options'">
                  <xsl:value-of select="concat($Symbol,'U')"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </IDCOOptionSymbol>-->




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

						<Quantity>
							<xsl:choose>
								<xsl:when test="number($Quantity)">
									<xsl:value-of select="$Quantity"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="Side" select="COL29"/>

						<Side>

							<xsl:choose>
								<xsl:when test="$Side='L'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:when test="$Side='S'">
									<xsl:value-of select="'Sell short'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>




						<xsl:variable name="MarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select=" COL30"/>
							</xsl:call-template>
						</xsl:variable>


						<MarkPrice>
							<xsl:choose>

								<xsl:when test="$MarkPrice &gt; 0">
									<xsl:value-of select="$MarkPrice"/>
								</xsl:when>

								<xsl:when test="$MarkPrice &lt; 0">
									<xsl:value-of select="$MarkPrice * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>


						</MarkPrice>

						<xsl:variable name="FXRate">
							<xsl:value-of select="COL46 "/>
						</xsl:variable>

						<FXRate>
							<xsl:choose>
								<xsl:when test="number($FXRate)">
									<xsl:value-of select="$FXRate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
						</FXRate>

						<xsl:variable name="varMarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL32"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varMarketValueSwap1">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL34"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varMarketValueSwap2">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL40"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varMarketValueSwap">
							<xsl:value-of select="$varMarketValueSwap1 - $varMarketValueSwap2"/>
						</xsl:variable>

						<MarketValue>
							<xsl:choose>
								<xsl:when test="contains(COL51,'EQUITY SWAP') or contains(COL51,'EQUITY SWAP FINANCING')">
									<xsl:choose>
										<xsl:when test="number($varMarketValueSwap)">
											<xsl:value-of select="$varMarketValueSwap"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="number($varMarketValue)">
											<xsl:value-of select="$varMarketValue"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>

						</MarketValue>

						<xsl:variable name="MarketValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL33"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="varMarketValueBaseSwap1">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL35"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varMarketValueBaseSwap2">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL41"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varMarketValueBaseSwap">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$varMarketValueBaseSwap1 - $varMarketValueBaseSwap2"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValueBase>
							<xsl:choose>
								<xsl:when test="contains(COL51,'EQUITY SWAP') or contains(COL51,'EQUITY SWAP FINANCING')">
									<xsl:choose>
										<xsl:when test="number($varMarketValueBaseSwap)">
											<xsl:value-of select="$varMarketValueBaseSwap"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="number($MarketValueBase)">
											<xsl:value-of select="$MarketValueBase"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValueBase>
						<xsl:variable name ="Bloomberg" select="COL14"/>

						<Bloomberg>
							<xsl:choose>
								<xsl:when test ="contains(COL14,'US')">
									<xsl:value-of select="concat($Bloomberg,' ','EQUITY')"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Bloomberg>



						<CompanyName>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</CompanyName>

						<xsl:variable name ="Date" select="COL1"/>


						<!--<xsl:variable name="Year1" select="substring-after(substring-after($Date,'/'),'/')"/>
            <xsl:variable name="Month" select="substring-before(substring-after($Date,'/'),'/')"/>
            <xsl:variable name="Day" select="substring-before($Date,'/')-->"/>



						<TradeDate>

							<!--<xsl:value-of select="concat($Month,'/',$Day,'/',$Year1)"/>-->
							<xsl:value-of select="$Date"/>
						</TradeDate>

						<SEDOL>
							<xsl:value-of select="normalize-space(COL9)"/>
						</SEDOL>

						<CUSIP>
							<xsl:value-of select="normalize-space(COL7)"/>
						</CUSIP>


						<CurrencySymbol>
							<xsl:value-of select="normalize-space(COL44)"/>
						</CurrencySymbol>

						<ISINSymbol>
							<xsl:value-of select="normalize-space(COL10)"/>
						</ISINSymbol>



					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>