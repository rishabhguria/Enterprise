<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
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

	<xsl:template name="Date">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month='JAN'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month='FEB'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month='MAR'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month='APR'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month='MAY'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month='JUN'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month='JUL'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month='AUG'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month='SEP'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month='OCT'">
				<xsl:value-of select="10"/>
			</xsl:when>
			<xsl:when test="$Month='NOV'">
				<xsl:value-of select="11"/>
			</xsl:when>
			<xsl:when test="$Month='DEC'">
				<xsl:value-of select="12"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="COL21='Equity Options'">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(substring-after(substring-after($Symbol,'-'),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after($Symbol,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="normalize-space(substring-before(substring-after($Symbol,'EXP'),'/'))"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(substring-after($Symbol,'/'),'/'),3,2)"/>
			</xsl:variable>
			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-before($Symbol,'-'),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring-before(substring-after(substring-after($Symbol,'@'),' '),'EXP'),'#.00')"/>
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

			<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
		</xsl:if>
	</xsl:template>
	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">
				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL8"/>
					</xsl:call-template>
				</xsl:variable>


				<xsl:if test="number($Quantity)">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'MSFS'"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:choose>
								<xsl:when test="COL42='Equity Swap'">
									<xsl:value-of select="concat(COL20,' ',COL42)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="(COL20)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name = "PB_SUFFIX_NAME" >
							<xsl:value-of select ="substring-after(COL46,'.')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="COL42='Option'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:when test="COL42='Cash Equity'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="COL42='Equity Swap'">
									<xsl:value-of select="'EquitySwap'"/>
								</xsl:when>
								<xsl:when test="COL42='Futures'">
									<xsl:value-of select="'Future'"/>
								</xsl:when>
								<xsl:when test="COL42='Option On Commodity'">
									<xsl:value-of select="'FutureOption'"/>
								</xsl:when>
								<xsl:when test="COL42='FX Forward'">
									<xsl:value-of select="'FXForward'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<!--<AssetType>
							<xsl:value-of select="$Asset"/>
						</PBAssetType>-->

						<xsl:variable name ="varFXMonths">
							<xsl:call-template name="Date">
								<xsl:with-param name="Month" select="substring-before(substring-after(COL20,'-'),'-')"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varFXDay">
							<xsl:value-of select="substring-before(substring-after(substring-after(COL20,':'),' '),'-')"/>
						</xsl:variable>

						<xsl:variable name="varFXYear">
							<xsl:value-of select="substring-after(substring-after(COL20,'-'),'-')"/>
						</xsl:variable>



						<xsl:variable name="varFXForward">
							<xsl:choose>
								<xsl:when test="COL17='EUR' or COL17='GBP' or COL17='NZD' or COL17='AUD'">
									<xsl:value-of select="concat(COL17,'/','USD',' ',$varFXMonths,'/',$varFXDay,'/',$varFXYear)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat('USD','/',COL17,' ',$varFXMonths,'/',$varFXDay,'/',$varFXYear)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>



						<xsl:variable name="varSymbol">
							<xsl:choose>
								<xsl:when test="contains(COL46,'.') and COL17 !='INR'">
									<xsl:value-of select="substring-before(COL46,'.')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL46"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Asset='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="COL20"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>

								<xsl:when test="$Asset='FXForward'">
									<xsl:value-of select="$varFXForward"/>
								</xsl:when>

								<xsl:when test="$Asset='Equity'">
									<xsl:value-of select="concat($varSymbol,$PRANA_SUFFIX_NAME)"/>
								</xsl:when>

								<xsl:when test="$Asset='EquityFutures'">
									<xsl:value-of select="COL20"/>
								</xsl:when>

								<xsl:when test="COL42='Equity Swap'">
									<xsl:value-of select="concat($varSymbol,$PRANA_SUFFIX_NAME)"/>
								</xsl:when>

								
								<xsl:when test="$varSymbol!='*'">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<!--<xsl:variable name="PB_FUND_NAME" select="concat(COL1,' ','-',' ',COL25)"/>-->

						<xsl:variable name = "PB_FUND_NAME" >
							<xsl:choose>
								<xsl:when test="COL21='Private Companies'">
									<xsl:value-of select="COL1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="concat(COL1,' ','-',' ',COL25)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name = $PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
							<xsl:value-of select="normalize-space(COL25)"/>
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
						</CounterPartyID>


						<xsl:variable name="varMonth">
							<xsl:value-of select="substring(COL6,5,2)"/>
						</xsl:variable>
						<xsl:variable name="varDay">
							<xsl:value-of select="substring(COL6,7,2)"/>
						</xsl:variable>

						<xsl:variable name="varYear">
							<xsl:value-of select="substring(COL6,1,4)"/>
						</xsl:variable>

						<TradeDate>
							<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
						</TradeDate>

						<OriginalPurchaseDate>
							<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
						</OriginalPurchaseDate>

						<SettlementDate>
							<xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
						</SettlementDate>


						<xsl:variable name="varNPosition">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL12"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varNetPosition">
							<xsl:choose>
								<xsl:when test="$Asset='FXForward' and (COL17='EUR' or COL17='GBP' or COL17='NZD' or COL17='AUD')">
										<xsl:value-of select="COL8"/>
								</xsl:when>

								<xsl:when test="$Asset='FXForward' and (COL17!='EUR' or COL17 !='GBP' or COL17 !='NZD' or COL17 !='AUD')">
									<xsl:value-of select="$varNPosition"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$Quantity"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Quantity>
							<xsl:choose>
								<xsl:when test="$Asset='FXForward'">
									<xsl:choose>
										<xsl:when test="$varNetPosition &gt; 0">
											<xsl:value-of select="$varNetPosition"/>
										</xsl:when>
										<xsl:when test="$varNetPosition &lt; 0">
											<xsl:value-of select="$varNetPosition * (-1)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="number($varNetPosition)">
											<xsl:value-of select="$varNetPosition"/>
										</xsl:when>
										
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
								
							</xsl:choose>
							
							
							
							
						</Quantity>

						


						<xsl:variable name="varSide">
							<xsl:value-of select="COL4"/>
						</xsl:variable>
						<Side>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:choose>
										<xsl:when test="$varSide ='L'">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>
										<xsl:when test="$varSide ='S'">
											<xsl:value-of select="'Sell to Close'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$varSide ='L'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when test="$varSide ='S'">
											<xsl:value-of select="'Sell short'"/>
										</xsl:when>
										<xsl:when test="$varSide ='n'">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>

						</Side>

						<xsl:variable name="varCOL8">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL8"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="varCOL28">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL28"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varCOL12">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL12"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varCOL16">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL16"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varCOL29">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL29"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varCOL40">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL40"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varCOL47">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL47"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varCostbasis">
							<xsl:choose>
								<xsl:when test="COL42='Cash Equity'">
									<xsl:value-of select="$varCOL16 div $varCOL8"/>
								</xsl:when>
								<!--<xsl:when test="COL42='Equity Swap'">
									<xsl:value-of select="$varCOL29 div $varCOL8"/>
								</xsl:when>-->

								<xsl:when test="COL42='Equity Swap'">
									<xsl:choose>
										<xsl:when test="COL21='Equity Options'">
											<xsl:value-of select="($varCOL29 div $varCOL8) div 100"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$varCOL29 div $varCOL8"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								
								<xsl:when test="COL42='Futures'">
									<xsl:value-of select="$varCOL29 div $varCOL8 div $varCOL47"/>
								</xsl:when>
								
								<xsl:when test="COL42='FX Forward'">
									<xsl:choose>
										<xsl:when test="COL17='EUR' or COL17='GBP' or COL17='NZD' or COL17='AUD'">
											<xsl:value-of select="$varCOL12 div $varCOL8"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$varCOL8 div $varCOL12"/>
										</xsl:otherwise>
									</xsl:choose>

								</xsl:when>
								<xsl:when test="COL42='Option'">
									<xsl:value-of select="($varCOL16 div $varCOL8) div 100 "/>
								</xsl:when>
								<xsl:when test="COL42='Option On Commodity'">
									<xsl:value-of select="$varCOL29 div $varCOL8 div $varCOL47"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varAvgPX">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$varCostbasis"/>
							</xsl:call-template>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="$varAvgPX &gt; 0">
									<xsl:value-of select="$varAvgPX"/>
								</xsl:when>
								<xsl:when test="$varAvgPX &lt; 0">
									<xsl:value-of select="$varAvgPX * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>


						<xsl:variable name="varFXRate">
							<xsl:choose>
								<xsl:when test="COL42='Cash Equity'">
									<xsl:value-of select="$varCOL12 div $varCOL16"/>
								</xsl:when>
								<xsl:when test="COL42='Equity Swap'">
									<xsl:value-of select="$varCOL28 div $varCOL29"/>
								</xsl:when>
								<xsl:when test="COL42='Futures'">
									<xsl:value-of select="$varCOL28 div $varCOL29"/>
								</xsl:when>
								<!--<xsl:when test="COL42='FX Forward'">
									<xsl:value-of select="$varCOL12 div $varCOL8"/>
								</xsl:when>-->



								<xsl:when test="COL42='FX Forward'">
									<xsl:choose>
										<xsl:when test="COL17='EUR' or COL17='GBP' or COL17='NZD' or COL17='AUD'">
											<xsl:value-of select="1"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$varCOL12 div $varCOL8"/>
										</xsl:otherwise>
									</xsl:choose>

								</xsl:when>


								<xsl:when test="COL42='Option'">
									<xsl:value-of select="$varCOL12 div $varCOL16"/>
								</xsl:when>
								<xsl:when test="COL42='Option On Commodity'">
									<xsl:value-of select="$varCOL28 div $varCOL29"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						


						<FXRate>
							<xsl:choose>
								<xsl:when test="number($varFXRate)">
									<xsl:value-of select="$varFXRate"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</FXRate>


						<xsl:variable name="varMarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL13"/>
							</xsl:call-template>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$varMarkPrice &gt; 0">
									<xsl:value-of select="$varMarkPrice"/>
								</xsl:when>
								<xsl:when test="$varMarkPrice &lt; 0">
									<xsl:value-of select="$varMarkPrice * (1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<xsl:variable name="MarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL14"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varMarket">
							<xsl:choose>
								<xsl:when test="$Asset='EquityFutures'">
									<xsl:value-of select="(COL8 * COL13 * COL44) -(COL8 * COL15 * COL44)"/>
								</xsl:when>
								<xsl:when test="COL42='Equity Swap'">
									<xsl:value-of select="$varCOL40 - $varCOL29"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$MarketValue"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<!--<xsl:variable name="varMarket">
							<xsl:choose>
								<xsl:when test="$Asset='EquityFutures' or COL42='Equity Swap'">
									<xsl:value-of select="$varCOL40 - $varCOL29"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$MarketValue"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>-->
						<MarketValue>
							<xsl:choose>
								<xsl:when test="number($MarketValue)">
									<xsl:value-of select="$MarketValue"/>
								</xsl:when>

							
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</MarketValue>


						<xsl:variable name="varCOL39">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL39"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="MarketValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL10"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varMarketValueBase">
							<xsl:choose>
								<xsl:when test="$Asset='EquityFutures' or COL42='Equity Swap'">
									<xsl:value-of select="$varCOL39 - $varCOL28"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$MarketValue"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<MarketValueBase>
							<xsl:choose>
								<xsl:when test="number($MarketValueBase)">
									<xsl:value-of select="$MarketValueBase"/>
								</xsl:when>								

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValueBase>

						<xsl:variable name="NetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL29"/>
							</xsl:call-template>
						</xsl:variable>

						<NetNotionalValue>

							<xsl:choose>

								<xsl:when test="$NetNotionalValue &gt; 0">
									<xsl:value-of select="$NetNotionalValue"/>
								</xsl:when>

								<xsl:when test="$NetNotionalValue &lt; 0">
									<xsl:value-of select="$NetNotionalValue * (1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</NetNotionalValue>

						<xsl:variable name="NetNotionalValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL28"/>
							</xsl:call-template>
						</xsl:variable>

						<NetNotionalValueBase>
							<xsl:choose>
								<xsl:when test="$NetNotionalValueBase &gt; 0">
									<xsl:value-of select="$NetNotionalValueBase"/>
								</xsl:when>
								<xsl:when test="$NetNotionalValueBase &lt; 0">
									<xsl:value-of select="$NetNotionalValueBase * (1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValueBase>

            <xsl:variable name="SettlCurrency">
              <xsl:choose>

                <xsl:when test="COL47='USD'">
                  <xsl:value-of select="'USD'"/>
                </xsl:when>
                <xsl:when test="COL47=''">
                  <xsl:value-of select="COL17"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="COL17"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <xsl:variable name="NetNotionalValueLocal">
              <xsl:choose>

                <xsl:when test="$NetNotionalValue &gt; 0">
                  <xsl:value-of select="$NetNotionalValue"/>
                </xsl:when>

                <xsl:when test="$NetNotionalValue &lt; 0">
                  <xsl:value-of select="$NetNotionalValue * (1)"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="NetNotionalValueBase3">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL28"/>
              </xsl:call-template>
            </xsl:variable>

            <xsl:variable name="NetNotionalValueBase2">

              <xsl:choose>
                <xsl:when test="$NetNotionalValueBase &gt; 0">
                  <xsl:value-of select="$NetNotionalValueBase3"/>
                </xsl:when>
                <xsl:when test="$NetNotionalValueBase &lt; 0">
                  <xsl:value-of select="$NetNotionalValueBase3 * (1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <SettlementCurrencyTotalCost>

              <xsl:choose>

                <xsl:when test="$SettlCurrency='USD'" >
                  <xsl:value-of select="$NetNotionalValueBase2"/>
                </xsl:when>

                <xsl:when test="$SettlCurrency!='USD'">
                  <xsl:value-of select="$NetNotionalValueLocal"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>

              </xsl:choose>

            </SettlementCurrencyTotalCost>


            <xsl:variable name="varSMarketValueLocala">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL14"/>
              </xsl:call-template>
            </xsl:variable>


            <xsl:variable name="varMarketValueLocala">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL10"/>
              </xsl:call-template>
            </xsl:variable>

            <SettlementCurrencyMarketValue>
              <xsl:choose>
                <xsl:when test="$SettlCurrency='USD'" >
                  <xsl:value-of select="$varMarketValueLocala"/>
                </xsl:when>

                <xsl:when test="$SettlCurrency!='USD'">
                  <xsl:value-of select="$varSMarketValueLocala"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>

            </SettlementCurrencyMarketValue>
						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>