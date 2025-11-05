<?xml version="1.0" encoding="utf-8"?>
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
  
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation"></xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="(COL3)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:variable name ="varSide">
					<xsl:value-of select ="translate(COL1,'&quot;','')"/>
				</xsl:variable>

				<xsl:if test="number($varQuantity)">
					<PositionMaster>
						<OrderSideTagValue>
							<xsl:choose>
								<xsl:when test="$varSide='Purchase'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$varSide='Buy'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sale'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="$varSide='Sell Short'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="$varSide='Buy to Close'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="$varSide='Buy To Close'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrderSideTagValue>
						

						<Symbol>
							<xsl:value-of select="normalize-space(COL2)"/>
						</Symbol>


						<Quantity>
							<xsl:choose>
								<xsl:when test="number($varQuantity)">
									<xsl:value-of select="number($varQuantity)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>
						
						<xsl:variable name ="varOrderTypeTagValue">
							<xsl:value-of select ="translate(normalize-space(COL4),'&quot;','')"/>
						</xsl:variable>
						<OrderTypeTagValue>
							<xsl:choose>
								<xsl:when test ="$varOrderTypeTagValue='Market'">
									<xsl:value-of select ="'1'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='Limit'">
									<xsl:value-of select ="'2'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='Stop'">
									<xsl:value-of select ="'3'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='Stop Limit'">
									<xsl:value-of select ="'4'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='Market on close'">
									<xsl:value-of select ="'5'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='With or without'">
									<xsl:value-of select ="'6'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='Limit or better'">
									<xsl:value-of select ="'7'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='Limit with or without'">
									<xsl:value-of select ="'8'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='On basis'">
									<xsl:value-of select ="'9'"/>
								</xsl:when>
								<xsl:when test ="$varOrderTypeTagValue='Pegged'">
									<xsl:value-of select ="'P'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="'1'"/>
								</xsl:otherwise>
							</xsl:choose>
						</OrderTypeTagValue>

						<!-- <xsl:variable name ="varPrice"> -->
							<!--  <xsl:value-of select ="translate(translate(COL5,'&quot;',''),',','')"/> -->
							<!-- <xsl:value-of select ="translate(COL5,$SingleQuote,'')"/> -->
						<!-- </xsl:variable> -->
						<xsl:variable name="varPrice">
					<xsl:call-template name="Translate">
					<xsl:with-param name="Number" select="(COL5)"/>
						</xsl:call-template>
						</xsl:variable>
		
						<Price>
							<xsl:choose>
								<xsl:when test ="contains($varOrderTypeTagValue,'Limit')">
									<xsl:value-of select="$varPrice"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Price>

						<Venue>
							<xsl:value-of select="'Drops'"/>
						</Venue>

						<VenueID>
							<xsl:value-of select="1"/>
						</VenueID>

						<CounterPartyName>
							<xsl:value-of select="'JPMS'"/>
						</CounterPartyName>

						<CounterPartyID>
							<xsl:value-of select="24"/>
						</CounterPartyID>

						<HandlingInstruction>
							<xsl:value-of select="3"/>
						</HandlingInstruction>

						
						<xsl:variable name ="varTIF">
							<xsl:value-of select ="translate(COL8,'&quot;','')"/>
						</xsl:variable>
						<TIF>							
							<xsl:choose>
								<xsl:when test ="$varTIF = 'Day' or $varTIF ='DAY'">
									<xsl:value-of select="0"/>
								</xsl:when>
								<xsl:when test ="$varTIF = 'Gtc' or $varTIF ='GTC'">
									<xsl:value-of select="1"/>
								</xsl:when>							
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TIF>

						<xsl:variable name ="varFund">
							<xsl:value-of select ="translate(normalize-space(COL6),'&quot;','')"/>
						</xsl:variable>
						<Level1ID>
							<xsl:choose>
								<xsl:when test ="$varFund = 'SCP Long Only Fund (account number: xxx9971)'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test ="$varFund = 'SCP Alpha Omega Fund, L.P.'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test ="$varFund = 'Alpha Omega LP'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test ="$varFund = 'SCP Equity Long Only Fund, L.P.'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test ="$varFund = 'Donald Smith JPM: 420-25518'">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test ="$varFund = 'SCP Endowment Fund Onshore (account number: xxx955)'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test ="$varFund = 'SCP Endowment Fund, L.P.'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test ="$varFund = 'Endowment Onshore'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test ="$varFund = 'Endow On CS: 6957-9962'">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test ="$varFund = 'SF JPM: 420-34446'">
									<xsl:value-of select="5"/>
								</xsl:when>
								<xsl:when test ="$varFund = 'MFs CS: 6710-2837'">
									<xsl:value-of select="6"/>
								</xsl:when>
								<xsl:when test ="$varFund = 'Gurtin CS: 2413-2055'">
									<xsl:value-of select="7"/>
								</xsl:when>
								<xsl:when test ="$varFund = 'SCP Endowment Fund Offshore (account number: xxx872)'">
									<xsl:value-of select="8"/>
								</xsl:when>
								<xsl:when test ="$varFund = 'SCP Endowment (Offshore) Fund, Ltd.'">
									<xsl:value-of select="8"/>
								</xsl:when>
								<xsl:when test ="$varFund = 'Endowment Offshore'">
									<xsl:value-of select="8"/>
								</xsl:when>
								<xsl:when test ="$varFund = 'Prorata'">
									<xsl:value-of select="10030"/>
								</xsl:when>
								<xsl:when test ="$varFund = '80/20 Split Onshore/Offshore'">
									<xsl:value-of select="10037"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="-2147483648"/>
								</xsl:otherwise>
						
							</xsl:choose>
						</Level1ID>

						<Level2ID>
								<xsl:value-of select ="0"/>
						</Level2ID>

						<TradingAccountID>
							<xsl:value-of select="11"/>
						</TradingAccountID>

						<ExecutionInstruction>
							<xsl:value-of select="'U'"/>
						</ExecutionInstruction>
						
						<CumQty>
							<xsl:value-of select="0"/>
						</CumQty>

						<PranaMsgType>
							<xsl:value-of select="3"/>
						</PranaMsgType>
						<UserID>
							<xsl:value-of select="'52'"/>								
						</UserID>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>