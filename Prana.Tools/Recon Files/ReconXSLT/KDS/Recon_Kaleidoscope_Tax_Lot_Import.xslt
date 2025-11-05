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

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">


				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL12"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity)">

					<PositionMaster>


						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'KLDS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="Symbol">
							<xsl:choose>
								<xsl:when test="string-length(COL38)=4">
									<xsl:value-of select="concat(substring(COL38,1,2),' ',substring(COL38,3,4))"/>
								</xsl:when>

								<xsl:when test="string-length(COL38)=5">
									<xsl:value-of select="concat(substring(COL38,1,3),' ',substring(COL38,4,5))"/>
								</xsl:when>

							</xsl:choose>
						</xsl:variable>

						<Symbol>

							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>


								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="normalize-space($Symbol)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>


						</Symbol>

					

						<xsl:variable name="PB_FUND_NAME" select="COL4"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="COL4='KALEIDOSCOPE PRISM MASTER FUND  LP'">
									<xsl:value-of select ="'MS'"/>
								</xsl:when>

								<xsl:when test ="COL4='KALEIDOSCOPE SPECTRUM MASTER FUND  LP'">
									<xsl:value-of select ="'KSMSFUT'"/>
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




						<xsl:variable name="Side" select="COL13"/>
						<Side>
							<xsl:choose>
										<xsl:when test="contains($Side,'L')">
											<xsl:value-of select="'1'"/>
										</xsl:when>

										<xsl:when test="contains($Side,'S')">
											<xsl:value-of select="'5'"/>
										</xsl:when>
							</xsl:choose>
						</Side>


						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>




						<xsl:variable name="COL18">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL18"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL14">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL14"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL17">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL17"/>
							</xsl:call-template>
						</xsl:variable>


						<xsl:variable name="Costbasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL15"/>
							</xsl:call-template>
						</xsl:variable>

						<UnitCost>
							<xsl:choose>
								<xsl:when test="$Costbasis &gt; 0">
									<xsl:value-of select="$Costbasis"/>
								</xsl:when>
								<xsl:when test="$Costbasis &lt; 0">
									<xsl:value-of select="$Costbasis * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</UnitCost>

						<xsl:variable name="MarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL23"/>
							</xsl:call-template>
						</xsl:variable>
						<MarketValue>
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
						</MarketValue>


						<xsl:variable name ="Date" select="COL5"/>


						<xsl:variable name="Year1" select="substring($Date,1,4)"/>
						<xsl:variable name="Month" select="substring($Date,5,2)"/>
						<xsl:variable name="Day" select="substring($Date,7,2)"/>

						<TradeDate>
							<xsl:value-of select="concat($Month,'/',$Day,'/',$Year1)"/>
						</TradeDate>
						
					
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>