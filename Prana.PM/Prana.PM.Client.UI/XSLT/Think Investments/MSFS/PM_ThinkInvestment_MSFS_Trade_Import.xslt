<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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
				<xsl:when test=" $Month=6">
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
		<xsl:param name="Suffix"/>
		<xsl:if test="contains(COL45,Options)">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(substring-after(substring-after(COL3,'-'),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(COL62,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(COL62,'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring-after(substring-after(COL62,'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-before(COL3,'-'),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(COL65,'#.00')"/>
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

	<xsl:template name="FutureCode">
		<xsl:param name="FMonth"/>
		<xsl:choose>
			<xsl:when test="$FMonth='JAN'">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$FMonth='FEB'">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$FMonth='MAR'">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$FMonth='APR'">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$FMonth='MAY'">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$FMonth='JUN'">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$FMonth='JUL'">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$FMonth='AUG'">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$FMonth='SEP'">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$FMonth='OCT'">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$FMonth='NOV'">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$FMonth='DEC'">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
		</xsl:choose>
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

			<xsl:for-each select ="//PositionMaster">


				<xsl:variable name="NetPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL14"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($NetPosition) and COL45 !='Cash'">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL3"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Asset">
							<xsl:choose>
								<xsl:when test="contains(COL45,'Options')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>

								<xsl:when test="COL45='Equities' and COL58='Equity Swap'">
									<xsl:value-of select="'EquitySwap'"/>
								</xsl:when>

								<xsl:when test="COL45='Equities'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="contains(COL45,'Futures')">
									<xsl:value-of select="'FUTURE'"/>
								</xsl:when>

								<xsl:when test="COL45='FX Forward' and COL58='FX Spot'">
									<xsl:value-of select="'FxSpot'"/>
								</xsl:when>

								<xsl:when test="contains(COL45,'FX Forward')">
									<xsl:value-of select="'FxForward'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<PBAssetType>
							<xsl:value-of select="$Asset"/>
						</PBAssetType>


						<xsl:variable name ="varFXMonths">
							<xsl:call-template name="Date">
								<xsl:with-param name="Month" select="substring-before(substring-after(COL3,'-'),'-')"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varFXDay">
							<xsl:value-of select="substring-before(substring-after(substring-after(COL3,':'),' '),'-')"/>
						</xsl:variable>

						<xsl:variable name="varFXYear">
							<xsl:value-of select="substring(substring-after(substring-after(COL3,'-'),'-'),3,2)"/>
						</xsl:variable>


						<xsl:variable name="varFXForward">
							<xsl:choose>
								<xsl:when test="COL32='EUR' or COL32='GBP' or COL32='NZD'">
									<xsl:value-of select="concat(COL32,'/','USD',' ',$varFXMonths,'/',$varFXDay,'/',$varFXYear)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat('USD','/',COL32,' ',$varFXMonths,'/',$varFXDay,'/',$varFXYear)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varFXSpot">
							<xsl:choose>
								<xsl:when test="COL32='EUR' or COL32='GBP' or COL32='NZD' or COCOL32L13='AUD'">
									<xsl:value-of select="concat(COL32,'/','USD')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat('USD','/',COL32)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varSEDOL">
							<xsl:value-of select="normalize-space(COL5)"/>
						</xsl:variable>

						<xsl:variable name = "PB_SUFFIX_NAME" >
							<xsl:value-of select ="substring-after(COL6,'.')"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="varFutureSymbol">
							<xsl:value-of select="substring-before(substring-after(normalize-space(COL3),' '),' ')"/>
						</xsl:variable>

						<xsl:variable name="varFutureYear">
							<xsl:value-of select="substring(substring-before(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),5,1)"/>
						</xsl:variable>

						<xsl:variable name ="varFutureMonthCode">
							<xsl:call-template name="FutureCode">
								<xsl:with-param name="FMonth" select="substring(substring-before(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),1,3)"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="varFuture">
							<xsl:choose>
								<xsl:when test="contains(COL45,'Futures') and COL32='INR'">
									<xsl:value-of select="concat($varFutureSymbol,' ',$varFutureMonthCode,$varFutureYear,'-NSF')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat($varFutureSymbol,' ',$varFutureMonthCode,$varFutureYear)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:variable name="varSymbol">
							<xsl:choose>
								<xsl:when test="contains(COL6,'.')">
									<xsl:value-of select="substring-before(COL6,'.')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL6"/>
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
										<xsl:with-param name="Suffix" select="''"/>
										<xsl:with-param name="Symbol" select="COL3"/>
									</xsl:call-template>
								</xsl:when>

								<xsl:when test="$Asset='FxSpot'">
									<xsl:value-of select="$varFXSpot"/>
								</xsl:when>

								<xsl:when test="$Asset='FxForward'">
									<xsl:value-of select="$varFXForward"/>
								</xsl:when>

								<xsl:when test="$Asset='EquitySwap'">
									<xsl:value-of select="concat($varSymbol,$PRANA_SUFFIX_NAME)"/>
								</xsl:when>

								<xsl:when test="$Asset='FUTURE'">
									<xsl:value-of select="$varFuture"/>
								</xsl:when>

								<xsl:when test="$Asset='Equity'">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<SEDOL>
							<xsl:value-of select="$varSEDOL"/>
						</SEDOL>

						<xsl:variable name="PB_FUND_NAME" select="''"/>

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
							<xsl:value-of select="COL23"/>
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


						<NetPosition>
							<xsl:choose>
								<xsl:when test="$NetPosition &gt; 0">
									<xsl:value-of select="$NetPosition"/>
								</xsl:when>
								<xsl:when test="$NetPosition &lt; 0">
									<xsl:value-of select="$NetPosition* (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="CostBasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL15"/>
							</xsl:call-template>
						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$CostBasis &gt; 0">
									<xsl:value-of select="$CostBasis"/>
								</xsl:when>
								<xsl:when test="$CostBasis &lt; 0">
									<xsl:value-of select="$CostBasis * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>


						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL28"/>
							</xsl:call-template>
						</xsl:variable>
						<Commission>
							<xsl:choose>
								<xsl:when test="$Asset='EquitySwap'">
									<xsl:value-of select="0"/>
								</xsl:when>
								<xsl:otherwise>
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
								</xsl:otherwise>
							</xsl:choose>

						</Commission>



						<xsl:variable name="varSecfee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL30"/>
							</xsl:call-template>
						</xsl:variable>
						<SecFee>
							<xsl:choose>
								<xsl:when test="$Asset='EquitySwap'">
									<xsl:value-of select="0"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$varSecfee &gt; 0">
											<xsl:value-of select="$varSecfee"/>

										</xsl:when>
										<xsl:when test="$varSecfee &lt; 0">
											<xsl:value-of select="$varSecfee * (-1)"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>

						</SecFee>


						<xsl:variable name ="Side" select="COL9"/>
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Asset='EquityOption'">
									<xsl:choose>
										<xsl:when test="$Side='BLX'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="$Side='BL'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="$Side='SL'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										<xsl:when test="$Side='SS'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$Side='BLX'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="$Side='BL'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="$Side='SL'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:when test="$Side='SS'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>

						</SideTagValue>


						<xsl:variable name="OtherBrokerFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="''"/>
							</xsl:call-template>
						</xsl:variable>

						<OtherBrokerFees>
							<xsl:choose>
								<xsl:when test="$OtherBrokerFee &gt; 0">
									<xsl:value-of select="$OtherBrokerFee * (-1)"/>

								</xsl:when>
								<xsl:when test="$OtherBrokerFee &lt; 0">
									<xsl:value-of select="$OtherBrokerFee * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</OtherBrokerFees>

						<xsl:variable name="FXRate">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL17"/>
							</xsl:call-template>
						</xsl:variable>
						<FXRate>
							<xsl:choose>
								<xsl:when test="number($FXRate)">
									<xsl:value-of select="$FXRate"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</FXRate>


						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>


						<PositionStartDate>
							<xsl:value-of select="COL11"/>
						</PositionStartDate>

						<PositionSettlementDate>
							<xsl:value-of select="COL12"/>
						</PositionSettlementDate>

						<xsl:if test="$Asset='EquitySwap'">

							<IsSwapped>
								<xsl:value-of select ="1"/>
							</IsSwapped>

							<SwapDescription>
								<xsl:value-of select ="'SWAP'"/>
							</SwapDescription>

							<DayCount>
								<xsl:value-of select ="365"/>
							</DayCount>

							<ResetFrequency>
								<xsl:value-of select ="'Monthly'"/>
							</ResetFrequency>

							<OrigTransDate>
								<xsl:value-of select ="COL11"/>
							</OrigTransDate>

							<xsl:variable name="varPrevMonth">
								<xsl:choose>
									<xsl:when test ="number(substring-before(COL11,'/')) = 1">
										<xsl:value-of select ="12"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="number(substring-before(COL11,'/'))-1"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>


							<xsl:variable name ="varSYear">
								<xsl:choose>
									<xsl:when test ="number(substring-before(COL11,'/')) = 1">
										<xsl:value-of select ="number(substring-after(substring-after(COL11,'/'),'/')) -1"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select ="number(substring-after(substring-after(COL11,'/'),'/'))"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<FirstResetDate>
								<xsl:value-of select ="concat($varPrevMonth,'/28/',$varSYear)"/>
							</FirstResetDate>
						</xsl:if>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>