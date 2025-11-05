<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
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

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 'Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Feb'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Mar'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Apr'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month = 'May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jun'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jul'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Aug'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Sep'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Oct'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Nov'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Dec'">
				<xsl:value-of select="'12'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>


	<xsl:template name ="varFutMonthCode">
		<xsl:param name="varFutMonth"/>

		<xsl:choose>

			<xsl:when  test ="$varFutMonth=1">
				<xsl:value-of select ="'F'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=2">
				<xsl:value-of select ="'G'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=3">
				<xsl:value-of select ="'H'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=4">
				<xsl:value-of select ="'J'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=5">
				<xsl:value-of select ="'K'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=6">
				<xsl:value-of select ="'M'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=7">
				<xsl:value-of select ="'N'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=8">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=9">
				<xsl:value-of select ="'U'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=10">
				<xsl:value-of select ="'v'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=11">
				<xsl:value-of select ="'X'"/>
			</xsl:when>

			<xsl:when  test ="$varFutMonth=12">
				<xsl:value-of select ="'Z'"/>
			</xsl:when>
		</xsl:choose>

	</xsl:template>

  <xsl:template name="Date">
    <xsl:param name="Month"/>
    <xsl:choose>
      <xsl:when test="$Month='Jan'">
        <xsl:value-of select="1"/>
      </xsl:when>
      <xsl:when test="$Month='Feb'">
        <xsl:value-of select="2"/>
      </xsl:when>
      <xsl:when test="$Month='Mar'">
        <xsl:value-of select="3"/>
      </xsl:when>
      <xsl:when test="$Month='Apr'">
        <xsl:value-of select="4"/>
      </xsl:when>
      <xsl:when test="$Month='May'">
        <xsl:value-of select="5"/>
      </xsl:when>
      <xsl:when test="$Month='Jun'">
        <xsl:value-of select="6"/>
      </xsl:when>
      <xsl:when test="$Month='Jul'">
        <xsl:value-of select="7"/>
      </xsl:when>
      <xsl:when test="$Month='Aug'">
        <xsl:value-of select="8"/>
      </xsl:when>
      <xsl:when test="$Month='Sep'">
        <xsl:value-of select="9"/>
      </xsl:when>
      <xsl:when test="$Month='Oct'">
        <xsl:value-of select="10"/>
      </xsl:when>
      <xsl:when test="$Month='Nov'">
        <xsl:value-of select="11"/>
      </xsl:when>
      <xsl:when test="$Month='Dec'">
        <xsl:value-of select="12"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>



  <xsl:template name="MonthCode">
    <xsl:param name="Month"/>
    <xsl:param name="PutOrCall"/>
    <xsl:if test="$PutOrCall='C'">
      <xsl:choose>
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'A'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'B'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'C'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'D'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'E'"/>
        </xsl:when>
        <xsl:when test=" $Month=06">
          <xsl:value-of select="'F'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'G'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'H'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
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
        <xsl:when test="$Month=01 ">
          <xsl:value-of select="'M'"/>
        </xsl:when>
        <xsl:when test="$Month=02 ">
          <xsl:value-of select="'N'"/>
        </xsl:when>
        <xsl:when test="$Month=03 ">
          <xsl:value-of select="'O'"/>
        </xsl:when>
        <xsl:when test="$Month=04 ">
          <xsl:value-of select="'P'"/>
        </xsl:when>
        <xsl:when test="$Month=05 ">
          <xsl:value-of select="'Q'"/>
        </xsl:when>
        <xsl:when test="$Month=06 ">
          <xsl:value-of select="'R'"/>
        </xsl:when>
        <xsl:when test="$Month=07  ">
          <xsl:value-of select="'S'"/>
        </xsl:when>
        <xsl:when test="$Month=08  ">
          <xsl:value-of select="'T'"/>
        </xsl:when>
        <xsl:when test="$Month=09 ">
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
    <xsl:if test="substring-before(COL15,' ') ='CALL' or substring-before(COL15,' ') ='PUT'">
      <xsl:variable name="UnderlyingSymbol">
        <xsl:value-of select="substring-before(substring-after(normalize-space(COL15),' '),' ')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryDay">
        <xsl:value-of select="substring-before(substring-after(normalize-space(COL15),'/'),'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryMonth">
        <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL15),' '),' '),'/')"/>
      </xsl:variable>
      <xsl:variable name="ExpiryYear">
        <xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL15),'/'),'/'),' ')"/>
      </xsl:variable>

      <xsl:variable name="PutORCall">
        <xsl:value-of select="substring(substring-before(COL15,' '),1,1)"/>
      </xsl:variable>
      <xsl:variable name="StrikePrice">
        <xsl:value-of select="format-number(substring-after(substring-after(substring-after(normalize-space(COL15),'/'),'/'),' '),'#.00')"/>
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


	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//Comparision">


				<xsl:variable name ="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL16"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test ="number($varQuantity) and COL8='B'">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'JP Morgan'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL15"/>
						</xsl:variable>

						<xsl:variable name="varCurrency">
							<xsl:value-of select="COL45"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varAsset">
							<xsl:choose>
								<xsl:when test="substring-before(COL15,' ')='CALL' or substring-before(COL15,' ')='PUT'">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Symbol">
							<xsl:choose>
								<xsl:when test="COL7='' or COL7='*'">
									<xsl:value-of select="substring(COL6,1,9)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(COL7)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>						
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
                <xsl:when test="$varAsset='EquityOption'">
                  <xsl:call-template name="Option">
                    <xsl:with-param name="Symbol" select="COL15"/>
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


						<xsl:variable name="PB_FUND_NAME" select="COL3"/>
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

			
						<xsl:variable name="Quantity">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL16 div 100000"/>
							</xsl:call-template>
						</xsl:variable>

						<Quantity>
							<xsl:choose>
								<xsl:when test="$varAsset='EquityOption'">
									<xsl:choose>
										<xsl:when test="number($Quantity) and COL17='S'">
											<xsl:value-of select="$Quantity *(-1)"/>
										</xsl:when>
										<xsl:when test="number($Quantity) and COL17='L'">
											<xsl:value-of select="$Quantity"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="number($Quantity)">
											<xsl:value-of select="$Quantity"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="MarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL33 div 100000000"/>
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


						<xsl:variable name="MarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL20 div 100"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValueBase>
							<xsl:choose>
								<xsl:when test="$MarketValue &gt; 0">
									<xsl:value-of select="$MarketValue"/>
								</xsl:when>
								<xsl:when test="$MarketValue &lt; 0">
									<xsl:value-of select="$MarketValue * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarketValueBase>



						<xsl:variable name="Side">
							<xsl:value-of select="normalize-space(COL17)"/>
						</xsl:variable>

						<Side>
									<xsl:choose>
										<xsl:when  test="$Side='L'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when  test="$Side='S'">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
							</xsl:choose>
						</Side>

						<TradeDate>
							<xsl:value-of select="''"/>
						</TradeDate>


						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>
						

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>

