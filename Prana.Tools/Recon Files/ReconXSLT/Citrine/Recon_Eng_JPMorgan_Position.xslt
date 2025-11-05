<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
				 xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="http://www.contoso.com">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="yes"/>

	<xsl:template name="tempMonthCodeCALL">
		<xsl:param name="paramMonthCodeCALL"/>
    
		<xsl:choose>
			<xsl:when test ="$paramMonthCodeCALL='01'">
				<xsl:value-of select= "'A'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='02'">
				<xsl:value-of select= "'B'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='03'">
				<xsl:value-of select= "'C'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='04'">
				<xsl:value-of select= "'D'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='05'">
				<xsl:value-of select= "'E'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='06'">
				<xsl:value-of select= "'F'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='07'">
				<xsl:value-of select= "'G'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='08'">
				<xsl:value-of select= "'H'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='09'">
				<xsl:value-of select= "'I'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='10'">
				<xsl:value-of select= "'J'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='11'">
				<xsl:value-of select= "'K'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodeCALL='12'">
				<xsl:value-of select= "'L'"/>
			</xsl:when>
				<xsl:otherwise>
				<xsl:value-of select= "' '"/>
			</xsl:otherwise>
		</xsl:choose>		
	</xsl:template>

	<xsl:template name="tempMonthCodePUT">
		<xsl:param name="paramMonthCodePUT"/>

		<xsl:choose>
			<xsl:when test ="$paramMonthCodePUT='01'">
				<xsl:value-of select= "'M'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='02'">
				<xsl:value-of select= "'N'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='03'">
				<xsl:value-of select= "'O'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='04'">
				<xsl:value-of select= "'P'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='05'">
				<xsl:value-of select= "'Q'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='06'">
				<xsl:value-of select= "'R'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='07'">
				<xsl:value-of select= "'S'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='08'">
				<xsl:value-of select= "'T'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='09'">
				<xsl:value-of select= "'U'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='10'">
				<xsl:value-of select= "'V'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='11'">
				<xsl:value-of select= "'W'"/>
			</xsl:when>
			<xsl:when test ="$paramMonthCodePUT='12'">
				<xsl:value-of select= "'X'"/>
			</xsl:when>
						<xsl:otherwise>
				<xsl:value-of select= "' '"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="/">
    <DocumentElement>
		
      <xsl:for-each select="//Comparision[substring(COL1,40,1) = 'O' or substring(COL1,40,1) = 'C' or substring(COL1,40,1) = 'A']">

        <xsl:variable name ="varPrice">
          <xsl:value-of select ="substring(COL1,235,16)"/>
        </xsl:variable>

        <xsl:variable name ="varPriceInt">
          <xsl:value-of select ="substring($varPrice,1,8)"/>
        </xsl:variable>

        <xsl:variable name ="varPriceFrac">
          <xsl:value-of select ="substring($varPrice,9,8)"/>
        </xsl:variable>

        <xsl:variable name ="varFormatPrice">
          <xsl:value-of select="concat($varPriceInt,'.',$varPriceFrac)"/>
        </xsl:variable>

        <xsl:variable name ="varNetPosition">
          
          <xsl:value-of select ="substring(COL1,94,14)"/>
        </xsl:variable>

        <xsl:variable name ="varQtyInt">
          <xsl:value-of select ="substring($varNetPosition,1,10)"/>
        </xsl:variable>


        <xsl:variable name ="varQtyFrac">
          <xsl:value-of select ="substring($varNetPosition,11,4)"/>
        </xsl:variable>

        <xsl:variable name ="varFormatQty">
          <xsl:value-of select="concat($varQtyInt,'.',$varQtyFrac)"/>
        </xsl:variable>


	  <xsl:variable name ="varExpiryYear">
		  <xsl:value-of select ="substring(COL1,79,2)"/>
	  </xsl:variable>
	  <xsl:variable name ="varStrikePriceINT">
		  <xsl:value-of select ="normalize-space(substring(COL1,83,4))"/>	  
	  </xsl:variable>
        <!--<xsl:variable name ="varStrikeDecimal">
          <xsl:choose>
            <xsl:when test ="$varStrikeDecimalPart != '' and string-length($varStrikeDecimalPart) = 1">
              <xsl:value-of select ="concat($varStrikeDecimalPart,'00')"/>
            </xsl:when>-->
	  <xsl:variable name ="varStrikePriceDEC1">      
		  <xsl:value-of select ="normalize-space(substring(COL1,87,2))"/>
	  </xsl:variable>

        <!--<xsl:variable name="varStrikePriceDEC">
          <xsl:choose>
            <xsl:when test="$varStrikePriceDEC= ''"/>
            <xsl:vslue-of select="$varStrikePriceDEC"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:vslue-of select="$varStrikePriceDEC"/>
            </xsl:otherwise>            
          </xsl:choose>
        </xsl:variable>-->
        <xsl:variable name="varStrikePriceDEC">
          <xsl:choose>
            <xsl:when test="$varStrikePriceDEC1=''">
              <xsl:value-of select="'00'"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$varStrikePriceDEC1"/>
            </xsl:otherwise>
          </xsl:choose>  
        </xsl:variable>
        
        <!--<A>
          <xsl:value-of select="substring(COL1,61,4)"/>
        </A>
        <B>
          <xsl:value-of select="substring(COL1,61,3)"/>
        </B>
        <C>
          <xsl:value-of select="substring(COL1,73,2)"/>
        </C>-->
	  <xsl:variable name ="varMonthCodeOption">
		  <xsl:choose>
			  <xsl:when test ="substring(COL1,61,4)='CALL'">
				  <xsl:call-template name="tempMonthCodeCALL">
					  <xsl:with-param name="paramMonthCodeCALL" select="substring(COL1,73,2)"/>
				  </xsl:call-template>		  
			  </xsl:when>
			  <xsl:when test ="substring(COL1,61,3)='PUT'">
				  <xsl:call-template name="tempMonthCodePUT">
					  <xsl:with-param name="paramMonthCodePUT" select="substring(COL1,73,2)"/>
				  </xsl:call-template>
			  </xsl:when>
			  <xsl:otherwise>
				  <xsl:value-of select ="''"/>
			  </xsl:otherwise>
		  </xsl:choose>
		  
	  </xsl:variable>

		  <xsl:if test="$varFormatPrice >= 0 and $varFormatQty > 0 and (substring(COL1,7,5) ='87017' or substring(COL1,7,5) ='87010' or substring(COL1,7,5) ='65501')">
			  <PositionMaster>

            <!--   Fund -->
            <xsl:variable name = "PB_FUND_NAME" >
              <xsl:if test ="substring(COL1,4,8)!= '-OF-F'">
                <xsl:value-of select="substring(COL1,7,5)"/>
              </xsl:if>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='Nirvana']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$PRANA_FUND_NAME=''">
                <AccountName>
                  <xsl:value-of select="''"/>
                </AccountName>
              </xsl:when>
              <xsl:otherwise>
                <AccountName>
                  <xsl:value-of select='$PRANA_FUND_NAME'/>
                </AccountName>
              </xsl:otherwise>
            </xsl:choose>

            <xsl:variable name="PB_COMPANY_NAME" select="normalize-space(substring(COL1,61,30))"/>

            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name='ML']/SymbolData[@PBCompanyName=$PB_COMPANY_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <Description>
              <xsl:value-of select ="$PB_COMPANY_NAME"/>
            </Description>

            <xsl:variable name ="varSymbol">
              <xsl:value-of select ="normalize-space(substring(COL1,26,8))"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="$PRANA_SYMBOL_NAME != ''">
                <Symbol>
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </Symbol>
              </xsl:when>
              <xsl:when test ="substring(COL1,40,1) = 'O'">
                <!--<xsl:variable name = "varLength" >
                  <xsl:value-of select="string-length($varSymbol)"/>
                </xsl:variable>-->
                <Symbol>
					<!--<xsl:value-of select="concat(substring($varSymbol,1,($varLength - 2)),' ',substring($varSymbol,($varLength - 1),$varLength))"/>-->
					<xsl:value-of select="concat('O:',$varSymbol,' ',$varExpiryYear,$varMonthCodeOption,$varStrikePriceINT,'.',$varStrikePriceDEC)"/>
                </Symbol>
              </xsl:when>
              <xsl:otherwise>
                <Symbol>
                  <xsl:value-of select="$varSymbol"/>
                </Symbol>
              </xsl:otherwise>
            </xsl:choose >

            <PBSymbol>
              <xsl:value-of select="substring(COL1,26,8)"/>
            </PBSymbol>

            <PBAssetName>
              <xsl:value-of select="''"/>
            </PBAssetName>

            <!--Side-->
            <xsl:variable name ="varSide">
              <xsl:value-of select="substring(COL1,109,1)"/>
            </xsl:variable>

            <Side>
            <xsl:choose>
              <xsl:when test="$varSide='L' and $varSide != '' ">
                  <xsl:value-of select="'Buy'"/>
              </xsl:when>
              <xsl:when test="$varSide='S' and $varSide != ''">
                  <xsl:value-of select="'Sell'"/>
              </xsl:when>
              <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
            </xsl:choose>
            </Side>

            <Quantity>
            <xsl:choose>
              <xsl:when test="$varSide='L' and number($varFormatQty)">
                  <xsl:value-of select="format-number($varFormatQty, '#,###.00000')"/>
              </xsl:when>
				<xsl:when test="$varSide='S' and number($varFormatQty)">
						<xsl:value-of select="format-number($varFormatQty * (-1),'#,###.00000')"/>
				</xsl:when>
              <xsl:otherwise>
                  <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </Quantity>

            <Markprice>
              <xsl:choose>
                <xsl:when test="boolean(number($varFormatPrice))">
                  <xsl:value-of select="format-number($varFormatPrice, '#,###.0000000')"/>
              </xsl:when>
              <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
              </Markprice>

          </PositionMaster>
        </xsl:if >
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>
