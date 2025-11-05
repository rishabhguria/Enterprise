<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">
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
	</xsl:template>

	<xsl:template name="FormatDate">
		<xsl:param name="DateTime" />

		<xsl:variable name="l">
			<xsl:value-of select="$DateTime + 68569 + 2415019" />
		</xsl:variable>

		<xsl:variable name="n">
			<xsl:value-of select="floor(((4 * $l) div 146097))" />
		</xsl:variable>

		<xsl:variable name="ll">
			<xsl:value-of select="$l - floor(((146097 * $n + 3) div 4))" />
		</xsl:variable>

		<xsl:variable name="i">
			<xsl:value-of select="floor(((4000 * ($ll + 1)) div 1461001))" />
		</xsl:variable>

		<xsl:variable name="lll">
			<xsl:value-of select="$ll - floor(((1461 * $i) div 4)) + 31" />
		</xsl:variable>

		<xsl:variable name="j">
			<xsl:value-of select="floor(((80 * $lll) div 2447))" />
		</xsl:variable>

		<xsl:variable name="nDay">
			<xsl:value-of select="$lll - floor(((2447 * $j) div 80))" />
		</xsl:variable>

		<xsl:variable name="llll">
			<xsl:value-of select="floor(($j div 11))" />
		</xsl:variable>

		<xsl:variable name="nMonth">
			<xsl:value-of select="floor($j + 2 - (12 * $llll))" />
		</xsl:variable>

		<xsl:variable name="nYear">
			<xsl:value-of select="floor(100 * ($n - 49) + $i + $llll)" />
		</xsl:variable>

		<xsl:variable name ="varMonthUpdated">
			<xsl:choose>
				<xsl:when test ="string-length($nMonth) = 1">
					<xsl:value-of select ="concat('0',$nMonth)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="$nMonth"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name ="nDayUpdated">
			<xsl:choose>
				<xsl:when test ="string-length($nDay) = 1">
					<xsl:value-of select ="concat('0',$nDay)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select ="$nDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:value-of select="$varMonthUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nDayUpdated"/>
		<xsl:value-of select="'/'"/>
		<xsl:value-of select="$nYear"/>

	</xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='JAN'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='FEB'">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='MAR'">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='APR'">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='MAY'">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='JUN'">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='JUL'">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='AUG'">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='SEP'">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='OCT'">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='NOV'">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='DEC'">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='JAN'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='FEB'">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='MAR'">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='APR'">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='MAY'">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='JUN'">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='JUL'">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='AUG'">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='SEP'">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='OCT'">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='NOV'">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='DEC'">
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
			<xsl:value-of select="substring-before(normalize-space(COL6),':')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring(substring-after(normalize-space(COL6),':'),1,2)"/>
		</xsl:variable>
		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="substring(substring-after(normalize-space(COL6),':'),3,3)"/>
		</xsl:variable>
		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring(substring-after(normalize-space(COL6),':'),8,2)"/>
		</xsl:variable>
		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(substring-after(substring-after(normalize-space(COL6),':'),':'),1,1)"/>
		</xsl:variable>
		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(substring-before(substring-after(substring-after(substring-after(normalize-space(COL6),':'),':'),':'),' '),'#.00')"/>
		</xsl:variable>
		<xsl:variable name="MonthCodVar">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="Month" select="$ExpiryMonth"/>
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
		<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ', $ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="varNetPosition">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL8"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varNetPosition) and (COL14='Equities' or COL14 ='Cash and Cash Equivalents') and normalize-space(COL29 )=''">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'NAV'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select="COL36"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varSEDOL">
							<xsl:value-of select="substring(normalize-space(COL27),2)"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varSEDOL !='' or $varSEDOL !='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>


						<SEDOL>
							<xsl:choose>
								<xsl:when test="$varSEDOL !='' or $varSEDOL !='*'">
									<xsl:value-of select="$varSEDOL"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SEDOL>


						<xsl:variable name="PB_FUND_NAME" select="COL2"/>
						
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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


						<xsl:variable name="varCostbasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL23"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="normalize-space(COL25)"/>
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


						<Quantity>
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
						</Quantity>

						<Asset>
							<xsl:choose>
								<xsl:when test="COL14 = 'Equities'">
									<xsl:value-of select="'Equity'"/>
								</xsl:when>
								<xsl:when test="COL14 = 'Cash and Cash Equivalents'">
									<xsl:value-of select="'Cash'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Asset>

						<xsl:variable name="varSide">
							<xsl:value-of select="normalize-space(COL13)" />
						</xsl:variable>

						<Side>
							<xsl:choose>
								<xsl:when test="$varSide = 'Sales'">
									<xsl:value-of select="'Sell'"/>
								</xsl:when>
								<xsl:when test="$varSide = 'Purchases'">
									<xsl:value-of select="'Buy'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="varTradeDate">
							<xsl:call-template name="FormatDate">
								<xsl:with-param name="DateTime" select="COL11"/>
							</xsl:call-template>
						</xsl:variable>
						
						<xsl:variable name="varSettlementDate">
							<xsl:call-template name="FormatDate">
								<xsl:with-param name="DateTime" select="COL10"/>
							</xsl:call-template>
						</xsl:variable>
						
						<TradeDate>
							<xsl:value-of select ="$varTradeDate"/>
						</TradeDate>				

						<SettlementDate>
							<xsl:value-of select ="$varSettlementDate"/>
						</SettlementDate>

						<xsl:variable name="varNetNotionalValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL23"/>
							</xsl:call-template>
						</xsl:variable>
						
						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="$varNetNotionalValue &gt; 0">
									<xsl:value-of select="$varNetNotionalValue"/>
								</xsl:when>
								<xsl:when test="$varNetNotionalValue &lt; 0">
									<xsl:value-of select="$varNetNotionalValue * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>


						<xsl:variable name="varNetNotionalValueBase">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL24"/>
							</xsl:call-template>
						</xsl:variable>
						<NetNotionalValueBase>
							<xsl:choose>
								<xsl:when test="$varNetNotionalValueBase &gt; 0">
									<xsl:value-of select="$varNetNotionalValueBase"/>
								</xsl:when>
								<xsl:when test="$varNetNotionalValueBase &lt; 0">
									<xsl:value-of select="$varNetNotionalValueBase * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValueBase>

                         <xsl:variable name="varComm">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL17"/>
              </xsl:call-template>
            </xsl:variable>
            <Commission>             
			       	<xsl:choose>
                <xsl:when test="$varComm &gt; 0">
                  <xsl:value-of select="$varComm"/>
                </xsl:when>
                <xsl:when test="$varComm &lt; 0">
                  <xsl:value-of select="$varComm * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>             
              </xsl:choose>
            </Commission>
			
			<xsl:variable name="varOtherBrokerFee">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL21"/>
              </xsl:call-template>
            </xsl:variable>
            <OtherBrokerFees>             
			       	<xsl:choose>
                <xsl:when test="$varOtherBrokerFee &gt; 0">
                  <xsl:value-of select="$varOtherBrokerFee"/>
                </xsl:when>
                <xsl:when test="$varOtherBrokerFee &lt; 0">
                  <xsl:value-of select="$varOtherBrokerFee * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>             
              </xsl:choose>
            </OtherBrokerFees>
						<CurrencySymbol>
							<xsl:value-of select="COL9"/>
						</CurrencySymbol>

						<xsl:variable name="PB_BROKER_NAME">
							<xsl:value-of select="normalize-space(COL31)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_BROKER_ID">
							<xsl:value-of select="document('../ReconMappingXml/CounterPartyMapping.xml')/CounterPartyMapping/PB[@Name=$PB_NAME]/CounterPartyData[@MappedBrokerCode=$PB_BROKER_NAME]/@BrokerCode"/>
						</xsl:variable>

						<CounterParty>
							<xsl:choose>
								<xsl:when test="number($PRANA_BROKER_ID)">
									<xsl:value-of select="$PRANA_BROKER_ID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_BROKER_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterParty>


						<SMRequest>
							<xsl:value-of select="'True'"/>
						</SMRequest>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>


</xsl:stylesheet>