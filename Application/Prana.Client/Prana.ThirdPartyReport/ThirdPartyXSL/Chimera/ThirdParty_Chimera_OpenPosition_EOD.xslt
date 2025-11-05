<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>

    <xsl:variable name="varYear">
      <xsl:value-of select="substring-before($Date,'-')"/>
    </xsl:variable>
    <xsl:variable name="varMonth">
      <xsl:value-of select="substring-before(substring-after($Date,'-'),'-')"/>
    </xsl:variable>
    <xsl:variable name="varDay">
      <xsl:value-of select="substring-after(substring-after($Date,'-'),'-')"/>
    </xsl:variable>
    <xsl:value-of select="concat($varMonth,'/',$varDay,'/',$varYear)"/>
  </xsl:template>

  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <ThirdPartyFlatFileDetail>
        <RowHeader>
          <xsl:value-of select ="'False'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>

        <AssetClass>
          <xsl:value-of select="'AssetClass'"/>
        </AssetClass>

        <LongShort>
          <xsl:value-of select="'LongShort'"/>
        </LongShort>

        <PutCall>
          <xsl:value-of select="'PutCall'"/>
        </PutCall>

        <Symbol>
          <xsl:value-of select="'Symbol'"/>
        </Symbol>

        <UnderlyingSymbol>
          <xsl:value-of select="'UnderlyingSymbol'"/>
        </UnderlyingSymbol>

        <Quantity>
          <xsl:value-of select="'Quantity'"/>
        </Quantity>

        <MarkPrice>
          <xsl:value-of select="'MarkPrice'"/>
        </MarkPrice>

        <Multiplier>
          <xsl:value-of select="'Multiplier'"/>
        </Multiplier>

        <MasterFundAccount>
          <xsl:value-of select="'MasterFundAccount'"/>
        </MasterFundAccount>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'False'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <AssetClass>
            <xsl:value-of select="AssetClass"/>
          </AssetClass>

          <LongShort>
            <xsl:value-of select="LongShort"/>
          </LongShort>

          <PutCall>
            <xsl:value-of select="PutCall"/>
          </PutCall>

          <Symbol>
            <xsl:value-of select="Symbol"/>
          </Symbol>

          <UnderlyingSymbol>
            <xsl:value-of select="UnderLyingSymbol"/>
          </UnderlyingSymbol>

          <Quantity>
            <xsl:value-of select="OpenPositions"/>
          </Quantity>

          <MarkPrice>
            <xsl:value-of select="MarkPrice"/>
          </MarkPrice>

          <Multiplier>
            <xsl:value-of select="Multiplier"/>
          </Multiplier>

          <MasterFundAccount>
            <xsl:value-of select="MasterFundAccount"/>
          </MasterFundAccount>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>